using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Players", fileName = "New Player Params")]
public class Player_Database : ScriptableObject
{
   [SerializeField, HideInInspector] private List<Player_Data> playerList;

    [SerializeField] private Player_Data currentPlayer;

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
        if (playerList == null)
            playerList = new List<Player_Data>();
        currentPlayer = new Player_Data();
        playerList.Add(currentPlayer);
        currentIndex = playerList.Count - 1;
    }
    public void RemoveElement()
    {
        if (currentIndex > 0)
        {
            currentPlayer = playerList[--currentIndex];
            playerList.RemoveAt(++currentIndex);
        }
        else
        {
            playerList.Clear();
            currentPlayer = null;
        }
    }
    public Player_Data GetNext()
    {
        if (currentIndex < playerList.Count - 1)
        {
            currentIndex++;
        }
        currentPlayer = this[currentIndex];
        return currentPlayer;

    }

    public Player_Data GetPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        currentPlayer = this[currentIndex];
        return currentPlayer;
    }

    public void ClearDatabase()
    {
        playerList.Clear();
        playerList.Add(new Player_Data());
        currentPlayer = playerList[0];
        currentIndex = 0;

    }

    public Player_Data GetRandomElement()
    {
        int random = Random.Range(0, playerList.Count);
        return playerList[random];
    }

    public Player_Data this[int index]
    {
        get
        {
            if (playerList != null && index>=0 && index < playerList.Count)
            {
                return playerList[index];
            }
            return null;
        }
        set
        {
            if (playerList == null)
            {
                playerList = new List<Player_Data>();
            }
            if (index >= 0 && index < playerList.Count && value != null)
            {
                playerList[index] = value;
            }
            else
            {
                Debug.LogError("Выход за границы массива, либо переданное значение = null");
            }

        }
    }
}

[System.Serializable]
public class Player_Data
{
    [Range(5,20)]
    [Tooltip("Здоровье")]
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

    [Range(0f, 5f)]
    [Tooltip("Величина ремонта")]
    [SerializeField] private float _repair;
    public float Repair
    {
        get
        {
            return _repair;
        }
    }


    [Tooltip("Префаб взрыва")]
    [SerializeField] private GameObject _boom;
    public GameObject Boom
    {
        get
        {
            return _boom;
        }
    }

    [Range(10,30)]
    [Tooltip("Максимальное колличество патронов")]
    [SerializeField] private int _bullet_value;
    public int BulletValue
    {
        get
        {
            return _bullet_value;
        }
        set
        {
            _bullet_value = value;
        }
    }

    [Range(0, 3)]
    [Tooltip("Величина прибавления патронов")]
    [SerializeField] private int _bullet_add;
    public int BulletAdd
    {
        get
        {
            return _bullet_add;
        }
    }

    [Range(0.1f,5f)]
    [Tooltip("Задержка между выстрелами")]
    [SerializeField] private float _delay;
    public float Delay
    {
        get
        {
            return _delay;
        }
    }

    [Tooltip("Скорость игрока")]
    [SerializeField] private float _speed;
    public float Speed
    {
        get
        {
            return _speed;
        }
    }

    [Tooltip("Ускорение игрока")]
    [SerializeField] private float _acceleration;
    public float Acceleration
    {
        get
        {
            return _acceleration;
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

    public Bullet_Data BulletData
    {
        get
        {

            return _bullet_database.GetElement(_bulletIndex); ;
        }
    }


    [Tooltip("Префаб игрока")]
    [SerializeField] private GameObject _player_prefab;
    public GameObject PlayerPrefab
    {
        get
        {
            return _player_prefab;
        }
    }

    [Tooltip("Минимальная сила базы данных игрока")]
    [Range(1,3)]
    [SerializeField] private int _player_hard_data=1;
    public int PlayerHardData
    {
        get
        {
            return _player_hard_data;
        }
    }




    public Player_Data()
    {

    }

    public Player_Data(Player_Data player_params)
    {
        _health = player_params.Health;
        _bulletIndex = player_params.BulletIndex;
        _player_hard_data = player_params.PlayerHardData;
        _bullet_database = player_params._bullet_database;
        _repair = player_params.Repair;
        _bullet_value = player_params.BulletValue;
        _bullet_add = player_params.BulletAdd;
        _delay = player_params.Delay;
        _boom = player_params.Boom;
        _player_prefab = player_params.PlayerPrefab;
        _speed = player_params.Speed;
        _acceleration = player_params.Acceleration;

    }
}