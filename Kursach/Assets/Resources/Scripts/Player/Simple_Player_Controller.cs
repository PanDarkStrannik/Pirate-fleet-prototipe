using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Simple_Player_Controller : MonoBehaviour, IShips
{
    [Tooltip("Максимальная сложность для контроллера противника")]
    [Range(1, 3)]
    [SerializeField] private int controller_hard = 1;

    [Range(1f,5f)]
    [Tooltip("Время до добавления снарядов")]
    [SerializeField] private float bullet_add_time = 1;


    [Range(3f, 7f)]
    [Tooltip("Время до добавления здоровья")]
    [SerializeField] private float health_add_time = 1;

  

    private NavMeshAgent _player_navigation;
    private Player_Data _data;

    public Player_Data Get_Data
    {
        get
        {
            return _data;
        }
    }


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
            int hard = controller_hard + _data.PlayerHardData;
            return hard;
        }
    }

    private Gun_Controller[] _guns;
    private int _max_bullets_value;
    private float _max_health_value;
    private GameObject _boom;

    private void Start()
    {        
        _boom=Instantiate(_data.Boom);
    }

    public void Initialize<Player_Data>(Player_Data data)
    {
        _player_navigation = gameObject.GetComponent<NavMeshAgent>();
        _guns = gameObject.GetComponentsInChildren<Gun_Controller>();


        _data = new global::Player_Data(data as global::Player_Data);
        _data.BulletData.Parent_Ship_ID = gameObject.GetInstanceID();


        _max_bullets_value = _data.BulletValue;
        _max_health_value = _data.Health;
        _player_navigation.speed = _data.Speed;
        _player_navigation.acceleration = _data.Acceleration;

        for (int i = 0; i < _guns.Length; i++)
        {
            _guns[i].Initialize(_data.BulletData);
        }
        var player_ui_init = (Player_UI)FindObjectOfType(typeof(Player_UI));
        player_ui_init.Player = _data;

        StartCoroutine(Shoot());
        StartCoroutine(BulletAdd());
        StartCoroutine(Repair());
    }

    private void FixedUpdate()
    {
        
        Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        transform.LookAt(new Vector3(mouse.origin.x, 0, mouse.origin.z));

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(mouse, out hit))
            {
                Vector3 mouse_position = new Vector3(hit.point.x, 0, hit.point.z);
                Navigation(mouse_position);
            }
        }
        
    }

    /// <summary>
    /// Указываем, что здоровье обнулилось и создаём взрыв
    /// </summary>
    public void Death()
    {

        _boom.transform.position = transform.position;
        _boom.GetComponent<ParticleSystem>().Play();   
        _data.Health = 0;
        gameObject.SetActive(false);
        Camera_Animation_Controller.Explouzion_Animation();

    }

    public void Navigation(Vector3 target)
    {
        _player_navigation.destination = target;
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (Input.GetMouseButton(0) && _data.BulletValue > 0)
            {
                for (int i = 0; i < _guns.Length; i++)
                {
                    _guns[i].Spawn();
                    _data.BulletValue--;
                    Camera_Animation_Controller.Shoot_Animation();
                }
                yield return new WaitForSeconds(_data.Delay);
            }          
        }
    }

    private IEnumerator BulletAdd()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            if (_data.BulletValue < _max_bullets_value)
            {
                yield return new WaitForSeconds(bullet_add_time);
                _data.BulletValue += _data.BulletAdd;
            }
            else
            {
                _data.BulletValue = _max_bullets_value;
          
            }

        }
    }

    public void TakeDamage(float damage)
    {
        _data.Health -= damage;
        Camera_Animation_Controller.Shoot_Animation();
        if (_data.Health <=0)
        {
            Death();
        }
    }

    private IEnumerator Repair()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (_data.Health < _max_health_value)
            {
                yield return new WaitForSeconds(health_add_time);
                _data.Health += _data.Repair;
            }
            else
            {
                _data.Health = _max_health_value;

            }

        }
    }

   
}
