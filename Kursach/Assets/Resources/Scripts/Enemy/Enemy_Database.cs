using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Enemies", fileName = "New Enemy Params")]
public class Enemy_Database : ScriptableObject
{
    [SerializeField, HideInInspector] private List<Enemy_Data> enemyList;

    [SerializeField] private Enemy_Data currentEnemy;

    private int currentIndex = 0;
    public int Index
    {
        get
        {
            return currentIndex;
        }
    }

    public void AddElement()
    {
        if (enemyList == null)
            enemyList = new List<Enemy_Data>();
        currentEnemy = new Enemy_Data();
        enemyList.Add(currentEnemy);
        currentIndex = enemyList.Count - 1;
    }
    public void RemoveElement()
    {
        if (currentIndex > 0)
        {
            currentEnemy = enemyList[--currentIndex];
            enemyList.RemoveAt(++currentIndex);
        }
        else
        {
            enemyList.Clear();
            currentEnemy = null;
        }
    }
    public Enemy_Data GetNext()
    {
        if (currentIndex < enemyList.Count - 1)
        {
            currentIndex++;
        }
        currentEnemy = this[currentIndex];
        return currentEnemy;

    }

    public Enemy_Data GetPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        currentEnemy = this[currentIndex];
        return currentEnemy;
    }

    public void ClearDatabase()
    {
        enemyList.Clear();
        enemyList.Add(new Enemy_Data());
        currentEnemy = enemyList[0];
        currentIndex = 0;

    }

    public Enemy_Data GetRandomElement()
    {
        int random = UnityEngine.Random.Range(0, enemyList.Count);
        return enemyList[random];
    }

    public Enemy_Data this[int index]
    {
        get
        {
            if (enemyList != null && index >= 0 && index < enemyList.Count)
            {
                return enemyList[index];
            }
            return null;
        }
        set
        {
            if (enemyList == null)
            {
                enemyList = new List<Enemy_Data>();
            }
            if (index >= 0 && index < enemyList.Count && value != null)
            {
                enemyList[index] = value;
            }
            else
            {
                Debug.LogError("Выход за границы массива, либо переданное значение = null");
            }

        }
    }
}

[System.Serializable]
public class Enemy_Data
{

    [Tooltip("Материал врага")]
    [SerializeField] private Material _enemy_material;
    /// <summary>
    /// Получение материала врага
    /// </summary>
    public Material Enemy_Material
    {
        get
        {
            return _enemy_material;
        }
    }

    [Tooltip("Здоровье")]
    [Range(0, 10)]
    [SerializeField] private float _health;
    /// <summary>
    /// Манипуляции со здоровьем
    /// </summary>
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }

    [Tooltip("Задержка между выстрелами")]
    [SerializeField] private float _delay;
    /// <summary>
    /// Получение задержки между выстрелами
    /// </summary>
    public float Delay
    {
        get
        {
            return _delay;
        }
    }

    [Tooltip("Дальность обнаружения")]
    [SerializeField] private float _detection_scale;
    public float Detection_Scale
    {
        get
        {
            return _detection_scale;
        }
    }

    [Tooltip("Скорость противника")]
    [SerializeField] private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
    }



    [Tooltip("База данных пуль")]
    [SerializeField] private Bullet_Database _bullet_database;

    [Tooltip("Вид используемых пуль")]
    [Range(0, 3)]
    [SerializeField] private int _bulletIndex = 0;
    public int BulletIndex
    {
        get
        {
            return _bulletIndex;
        }
    }

    [Tooltip("Префаб взрыва противника")]
    [SerializeField] private GameObject _enemy_expousion;
    public GameObject EnemyExplousion
    {
        get
        {
            return _enemy_expousion;
        }
    }

    [Tooltip("Сложность противника")]
    [Range(1, 15)]
    [SerializeField] private int _enemyHardData = 1;
    public int EnemyHardData
    {
        get
        {
            return _enemyHardData;
        }
    }


    public Bullet_Data BulletData
    {
        get
        {
            var bullet_data = _bullet_database.GetElement(_bulletIndex);
            return bullet_data;
        }
    }



    private List<Transform> _targets;
    public List<Transform> Targets
    {
        set
        {
            _targets = value;
        }
        get
        {
            return _targets;
        }
    }

    

    public Enemy_Data()
    {

    }

    public static Action<GameObject> Enemy_Death;

    public Enemy_Data(Enemy_Data enemy_params)
    {
        _health = enemy_params.Health;
        _bulletIndex = enemy_params.BulletIndex;
        _targets = enemy_params.Targets;
        _enemyHardData = enemy_params.EnemyHardData;
        _bullet_database = enemy_params._bullet_database;
        _enemy_material = enemy_params.Enemy_Material;
        _delay = enemy_params.Delay;
        _detection_scale = enemy_params.Detection_Scale;
        _enemy_expousion = enemy_params.EnemyExplousion;
        _speed = enemy_params.Speed;
      
    }
}

