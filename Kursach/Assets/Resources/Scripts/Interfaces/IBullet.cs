
using UnityEngine;


interface IBullet
{
    
    Bullet_Data Params
    {
        set;
        get;
    }


    void Initialize(Bullet_Data bulletData);

    void Movement();

    void Attack(Collider target);
    

}
