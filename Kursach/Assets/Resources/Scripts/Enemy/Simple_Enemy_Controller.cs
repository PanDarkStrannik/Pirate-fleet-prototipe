using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Simple_Enemy_Controller : MonoBehaviour, IShips
{
    [Tooltip("Сложность контроллера противника")]
    [Range(1, 3)]
    [SerializeField] private int controller_hard=1;

    protected NavMeshAgent _navigation;
    protected Gun_Controller []_guns;
    private Text _health_view;    
    private bool not_first_init = false;
    protected Enemy_Data _data;
    


    public int ShipID
    {
        get
        {
            return gameObject.GetInstanceID();
        }
    }

    public int ControllerHard
    {
        get
        {
            int hard = controller_hard * _data.EnemyHardData;
            return hard;
        }
    }
    
   
    private void OnEnable()
    {
        if (not_first_init)
        {
            int randomTarget = Random.Range(0, _data.Targets.Count);
            Navigation(_data.Targets[randomTarget].position);


            StartCoroutine(Shoot());
        }

    }


    protected virtual void FixedUpdate()
    {
        _health_view.text = _data.Health.ToString();
        if (transform.position.z <= -Border.BorderHeigth-1f || transform.position.x >= Border.BorderWidth
            || transform.position.x <= -Border.BorderWidth)
        {
            Death();
        }
    }



    public virtual void Navigation(Vector3 target)
    {
        _navigation.destination = target;
    }

    public IEnumerator Shoot()
    {
        List<Ray> rays = new List<Ray>();
        RaycastHit hit;
        List<bool> if_shoot_to_player = new List<bool>();


        for (int i = 0; i < _guns.Length; i++)
        {
            rays.Add(new Ray(_guns[i].gameObject.transform.position, transform.forward));
            if_shoot_to_player.Add(false);         
        }

       

        while (true)
        {
            if (if_shoot_to_player.TrueForAll(x => x == true))
            {
                for (int i = 0; i < _guns.Length; i++)
                {
                    if_shoot_to_player[i] = false;
                }
                yield return new WaitForSeconds(_data.Delay);
            }
           

            for (int i = 0; i < _guns.Length; i++)
            {

                rays[i]=new Ray(_guns[i].transform.position, transform.forward);
                Debug.DrawRay(_guns[i].gameObject.transform.position, transform.forward);


                if (Physics.Raycast(rays[i],out hit, _data.Detection_Scale))
                {
              
                    if (hit.collider.gameObject.CompareTag("Player") && if_shoot_to_player[i] == false)
                    {
                        _guns[i].Spawn();
                        if_shoot_to_player[i] = true;
                    }
                }
            }
            yield return new WaitForFixedUpdate();           
        }
    }

    public void TakeDamage(float damage)
    {
        _data.Health -= damage;
        if (_data.Health <= 0)
        {
            var explousion = Instantiate(_data.EnemyExplousion);
            explousion.transform.position = transform.position;
            explousion.GetComponent<ParticleSystem>().Play();
            Destroy(explousion,2f);
            Death();
            Camera_Animation_Controller.Explouzion_Animation();
        }
    }
    public void Death()
    {

        Enemy_Data.Enemy_Death(gameObject);
    }

    public void Initialize<Enemy_Data>(Enemy_Data data)
    {
        if (!not_first_init)
        {
            _navigation = gameObject.GetComponent<NavMeshAgent>();
            _guns = gameObject.GetComponentsInChildren<Gun_Controller>();
            _health_view = gameObject.GetComponentInChildren<Text>();



           

        }


        _data = new global::Enemy_Data(data as global::Enemy_Data);
        _data.BulletData.Parent_Ship_ID = gameObject.GetInstanceID();
        gameObject.GetComponentInChildren<MeshRenderer>().material = _data.Enemy_Material;
        _navigation.speed = _data.Speed;
        


        for (int i = 0; i < _guns.Length; i++)
        {
            _guns[i].Initialize(_data.BulletData);
        }

      

        not_first_init = true;
    }

   

}
