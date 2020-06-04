using System.Collections;
using UnityEngine;

interface IShips
{
    int ControllerHard
    {
        get;
    }

    int ShipID
    {
        get;
    }

    void TakeDamage(float damage);
    void Death();

    void Navigation(Vector3 target);

    IEnumerator Shoot();

    void Initialize<T>(T data);
 
}
