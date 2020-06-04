using UnityEngine;
using System.Collections;

public class Stalkered_Enemy_Controller : Simple_Enemy_Controller
{
    [Range(0f, 15f)]
    [Tooltip("Радиус сферического луча обнаружения")]
    [SerializeField] private float _triggered_sphere_radius = 1f;

    [Range(0.1f, 3f)]
    [Tooltip("Множитель увеличения длины обнаружения луча")]
    [SerializeField] private float _sphere_destination_factor = 1f;

    [Range(1f, 5f)]
    [Tooltip("Задержки между проверками на отказ от преследования")]
    [SerializeField] private float _fail_check_time = 1f;

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
                _is_player_detect = hit.collider.gameObject;
                _player = hit.collider.gameObject.transform;
                _is_player_detect = true;
               
            }

        }
        if (_is_player_detect)
        {

            Navigation(_player.position);
            StartCoroutine(_Fail_Stalking());

        }
        base.FixedUpdate();
    }

    private IEnumerator _Fail_Stalking()
    {
        yield return new WaitForSeconds(_fail_check_time);
        int check = Random.Range(0,10);
        if (check > 6)
        {
            _is_player_detect = false;
            int randomTarget = Random.Range(0, _data.Targets.Count);
            Navigation(_data.Targets[randomTarget].position);
        }

    }

}
