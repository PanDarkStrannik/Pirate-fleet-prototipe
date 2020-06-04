using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Controller : MonoBehaviour
{
    [Tooltip("Максимальное количество префабов пуль")]
    [SerializeField] private int bullet_value = 4;

    [Tooltip("Префаб пули")]
    [SerializeField] private GameObject bullet_prefab;

    private Bullet_Data _data;
   

    /// <summary>
    /// Очередь пуль для возвращения
    /// </summary>
    private Queue<GameObject> _bullets_queue;

    /// <summary>
    /// Эффект выстрела пушки
    /// </summary>
    private ParticleSystem _gun_effect;

    private AudioSource _gun_sound;

    private bool first_start = true;


    

    private void OnEnable()
    {
        Bullet_Data.Bullet_Destroy += _ReturnBullet;
    }

    private void OnDisable()
    {
        Bullet_Data.Bullet_Destroy -= _ReturnBullet;
    }
    public void Spawn()
    {
        if (_bullets_queue.Count > 0)
        {
            _gun_effect.Play();
            _gun_sound.Play();
            GameObject spawned_bullet = _bullets_queue.Dequeue();

            spawned_bullet.transform.rotation = transform.rotation;
            
            spawned_bullet.transform.position = transform.position;
            spawned_bullet.SetActive(true);
        }
    }

    public void Initialize(Bullet_Data bulletData)
    {
        _data = bulletData;
    
        if (first_start)
        {
            
            _bullets_queue = new Queue<GameObject>();
            _gun_effect = GetComponentInChildren<ParticleSystem>();
            _gun_sound = GetComponent<AudioSource>();
                       

            for (int i = 0; i < bullet_value; i++)
            {
                var bullet = Instantiate(bullet_prefab);
                _bullets_queue.Enqueue(bullet);
                bullet.SetActive(false);
         
            }

            first_start = false;
        }

        for (int i = 0; i < bullet_value; i++)
        {
           
            var bullet = _bullets_queue.Dequeue();
            var script = new Bullet_Data(_data);
            bullet.GetComponent<IBullet>().Initialize(script);
            _bullets_queue.Enqueue(bullet);
        }

        


    }


    private void _ReturnBullet(GameObject returned_bullet)
    {
        if (returned_bullet.GetComponent<IBullet>().Params.Parent_Ship_ID == GetComponentInParent<IShips>().ShipID
            && _bullets_queue.Count <= bullet_value)
        {
            returned_bullet.SetActive(false);
            _bullets_queue.Enqueue(returned_bullet);
        }
    }




}
