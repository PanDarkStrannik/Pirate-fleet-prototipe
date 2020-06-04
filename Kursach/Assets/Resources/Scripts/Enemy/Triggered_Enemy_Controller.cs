using System.Collections;
using UnityEngine;

public class Triggered_Enemy_Controller : Simple_Enemy_Controller
{
    [Range(0f,3f)]
    [Tooltip("Радиус сферического луча обнаружения")]
    [SerializeField] private float _triggered_sphere_radius=1f;

    [Range(0.1f, 3f)]
    [Tooltip("Множитель увеличения длины обнаружения луча")]
    [SerializeField] private float _sphere_destination_factor=1f;

    [Range(1f, 5f)]
    [Tooltip("Задержки между проверками на отказ от прицеливания")]
    [SerializeField] private float _fail_check_time = 2f;

    private bool _is_player_detect = false;
    private Transform _player;

    protected override void FixedUpdate()
    {
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, _triggered_sphere_radius, out hit, _data.Detection_Scale * _sphere_destination_factor)) 
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                _player = hit.collider.gameObject.transform;
                _is_player_detect=true;
               
            }
        }
        if(_is_player_detect)
        {
            transform.LookAt(_player);
            StartCoroutine(_Fail_Trigger());
        }
        base.FixedUpdate();
    }

    private IEnumerator _Fail_Trigger()
    {
        yield return new WaitForSeconds(_fail_check_time);
        int check = Random.Range(0, 10);
        if (check > 6)
        {
            _is_player_detect = false;      
        }

    }

}
