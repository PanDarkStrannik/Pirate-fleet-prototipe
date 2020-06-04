using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Controller : MonoBehaviour
{
    [Tooltip("Префабы игрока (с разными контролерами)")]
    [SerializeField] private List<GameObject> player_prefabs;

    [Tooltip("База данных с настройками игрока")]
    [SerializeField] private Player_Database player_database;

    [Tooltip("Префабы врага (с разными контролерами)")]
    [SerializeField] private List<GameObject> enemy_prefabs;

    [Tooltip("Колличество врагов для каждого префаба")]
    [SerializeField] private List<int> enemy_prefabs_value;

    [Tooltip("База данных с настройками противника")]
    [SerializeField] private Enemy_Database enemy_database;

    [Tooltip("Лист точек стремления для противников")]
    [SerializeField] private List<Transform> enemy_targets;

    [Tooltip("Продолжительность игры")]
    [SerializeField] private float game_time=60;

    [Tooltip("Задержка перед спавном нового врага")]
    [SerializeField] private float enemy_spawn_time=1;


    [Tooltip("Ссылка на Player_UI")]
    [SerializeField] private Player_UI player_ui;

 
    private Queue<GameObject> _enemy_queue;
    private GameObject _player;
    private int _max_enemy_on_scene=0;
    private int enemy_on_scene_count;
    




 
    private void Awake()
    {
        _Player_Spawn();
    }
    private void Start()
    {



        _enemy_queue = new Queue<GameObject>();
        for (int x = 0; x < enemy_prefabs.Count; x++)
        {
            for (int y = 0; y < enemy_prefabs_value[x]; y++)
            {
                var enemy = Instantiate(enemy_prefabs[x]);
           
                _enemy_queue.Enqueue(enemy);
                _Enemy_Init(enemy);
            
                enemy.SetActive(false);
                _max_enemy_on_scene++;
            }

        }

      
        StartCoroutine(_Spawner());
        Enemy_Data.Enemy_Death += _Return_Enemy;
    }



    private void _Player_Spawn()
    {
        

        var player_data = new Player_Data(player_database.GetRandomElement());
        _player = Instantiate(player_data.PlayerPrefab);
        _player.GetComponent<IShips>().Initialize(player_data);

        

        _player.transform.position = new Vector3(0, 0, 0);

       
    }

    private void _Enemy_Init(GameObject enemy)
    {
        var enemy_data = new Enemy_Data(enemy_database.GetRandomElement());
        enemy_data.Targets = enemy_targets;
        enemy.GetComponent<IShips>().Initialize(enemy_data);
    }


    private IEnumerator _Spawner()
    {
        enemy_on_scene_count = 0;
        int time = 0;
        while (time <= game_time)
        {
            if (_enemy_queue.Count > 0)
            {
                if (time < game_time / 5)
                {
                    if (enemy_on_scene_count <= 2)
                    {
                        _Harder_Check(1);
                    }
                    else
                    {
                        yield return new WaitForSeconds(enemy_spawn_time);
                        _Harder_Check(1.5f);
                    }
                }
                else if (time < game_time * 2 / 5)
                {
                    if (enemy_on_scene_count < 4)
                    {
                        _Harder_Check(1.5f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(enemy_spawn_time);
                        _Harder_Check(2);
                    }
                }

                else if (time < game_time * 3 / 5)
                {
                    if (enemy_on_scene_count < _max_enemy_on_scene / 2)
                    {
                        _Harder_Check(2);
                    }
                    else
                    {
                        var min_spawn_time = enemy_spawn_time / 2;
                        if (min_spawn_time > 2.5)
                        {
                            yield return new WaitForSeconds(enemy_spawn_time);
                        }
                        else
                        {
                            yield return new WaitForSeconds(2.5f);
                        }
                        _Harder_Check(2.5f);
                    }
                }
                else if (time < game_time && time != game_time)
                {
                    if (enemy_on_scene_count < _max_enemy_on_scene)
                    {
                        if (enemy_on_scene_count < _max_enemy_on_scene / 2)
                        {
                            _Harder_Check(2.5f);
                        }
                        else
                        {
                            var min_spawn_time = enemy_spawn_time / 4;
                            if (min_spawn_time > 0.25 && min_spawn_time < 1.5f)
                            {
                                yield return new WaitForSeconds(min_spawn_time);
                            }
                            else
                            {
                                yield return new WaitForSeconds(0.25f);
                            }
                            _Harder_Check(3);
                        }
                    }
                }
                else if (time == game_time)
                {
                    player_ui.Win();
                }

            }
            time++;
            yield return new WaitForSeconds(1);
            
        }
    }


    private void _Harder_Check(float complexity)
    {
        var spawned_enemy = _enemy_queue.Dequeue();
        var player_max_hard = _player.GetComponent<IShips>().ControllerHard * complexity;
        var player_min_hard = player_max_hard / 2;
        var spawned_enemy_hard = spawned_enemy.GetComponent<IShips>().ControllerHard;
      
        if (spawned_enemy_hard <= player_max_hard && spawned_enemy_hard >= player_min_hard)
        {
            float random_positionX = Random.Range(-Border.BorderWidth, Border.BorderWidth);
            spawned_enemy.transform.position = new Vector3(random_positionX, 0, Border.BorderHeigth + 2);
            spawned_enemy.transform.rotation = transform.rotation;
            spawned_enemy.SetActive(true);
            enemy_on_scene_count++;
        }
        else
        {
            _Enemy_Init(spawned_enemy);
            _enemy_queue.Enqueue(spawned_enemy);
            _Harder_Check(complexity);
        }
    }


    private void _Return_Enemy(GameObject returned_enemy)
    {
        
        _enemy_queue.Enqueue(returned_enemy);        
        returned_enemy.SetActive(false);        
        _Enemy_Init(returned_enemy);
        enemy_on_scene_count--;

    }

}


