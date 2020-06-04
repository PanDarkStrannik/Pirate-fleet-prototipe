using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Database/Bullets", fileName = "New Bullet Params")]
public class Bullet_Database : ScriptableObject
{
    [SerializeField, HideInInspector] private List<Bullet_Data> bulletList;

    [SerializeField] private Bullet_Data currentBullet;

    
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
        if (bulletList == null)
            bulletList = new List<Bullet_Data>();
        currentBullet = new Bullet_Data();
        bulletList.Add(currentBullet);
        currentIndex = bulletList.Count - 1;
    }
    public void RemoveElement()
    {
        if (currentIndex > 0)
        {
            currentBullet = bulletList[--currentIndex];
            bulletList.RemoveAt(++currentIndex);
        }
        else
        {
            bulletList.Clear();
            currentBullet = null;
        }
    }
    public Bullet_Data GetNext()
    {
        if (currentIndex < bulletList.Count - 1)
        {
            currentIndex++;
        }
        currentBullet = this[currentIndex];
        return currentBullet;

    }

    public Bullet_Data GetPrev()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        currentBullet = this[currentIndex];
        return currentBullet;
    }

    public void ClearDatabase()
    {
        bulletList.Clear();
        bulletList.Add(new Bullet_Data());
        currentBullet = bulletList[0];
        currentIndex = 0;

    }

 

    public Bullet_Data GetElement(int number)
    {
        if (number < bulletList.Count && number > 0)
        {
            return bulletList[number];
        }
        else
        {
            return bulletList[0];
        }
    }

    public Bullet_Data this[int index]
    {
        get
        {
            if (bulletList != null && index >= 0 && index < bulletList.Count)
            {
                return bulletList[index];
            }
            return null;
        }
        set
        {
            if (bulletList == null)
            {
                bulletList = new List<Bullet_Data>();
            }
            if (index >= 0 && index < bulletList.Count && value != null)
            {
                bulletList[index] = value;
            }
            else
            {
                Debug.LogError("Выход за границы массива, либо переданное значение = null");
            }

        }
    }
}

[System.Serializable]
public class Bullet_Data
{
    
    [Tooltip("Урон снаряда")]
    [SerializeField] private float _damage;
    /// <summary>
    /// Получение величины урона
    /// </summary>
    public float Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }

    [Tooltip("Скорость снаряда")]
    [SerializeField] private float _speed;
    /// <summary>
    /// Получение скорости
    /// </summary>
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _damage = value;
        }
    }


    [Tooltip("Материал снаряда")]
    [SerializeField] private Material _bullet_material;
    public Material Bullet_Material
    {
        get
        {
            return _bullet_material;
        }
    }

    

    private int _parent_ship_id;
    /// <summary>
    /// ID материнского корабля для пули
    /// </summary>
    public int Parent_Ship_ID
    {
        get
        {
            return _parent_ship_id;
        }
        set
        {
            _parent_ship_id = value;
        }
    }

    public static Action<GameObject> Bullet_Destroy;


    public Bullet_Data()
    {

    }
    public Bullet_Data(Bullet_Data data)
    {
        _damage = data.Damage;
        _speed = data.Speed;
        _bullet_material = data.Bullet_Material;
        _parent_ship_id = data.Parent_Ship_ID;
    }

    

}
