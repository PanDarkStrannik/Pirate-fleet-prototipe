using UnityEngine;


public class Simple_Bullet_Controller : MonoBehaviour, IBullet
{
    protected Rigidbody _bullet_body;
    
    protected Bullet_Data _data;
    public Bullet_Data Params
    {
        set
        {
            _data = value;
        }
        get
        {
            return _data;
        }
    }
    protected bool first_init = true;

    public virtual void Initialize(Bullet_Data bulletData)
    {
        _data = bulletData;
        if (first_init)
        {
            _bullet_body = gameObject.GetComponent<Rigidbody>();
            gameObject.GetComponent<MeshRenderer>().material = _data.Bullet_Material;
            first_init = false;
        }

    }

   


    private void OnEnable()
    {
        if (!first_init)
        {
            Movement();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (transform.position.z >= Border.BorderHeigth || transform.position.z <= -Border.BorderHeigth
            || transform.position.x >= Border.BorderWidth || transform.position.x <= -Border.BorderWidth)
        {
            Bullet_Data.Bullet_Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
       Attack(other);
    }



    public virtual void Movement()
    {
        _bullet_body.velocity = transform.forward * _data.Speed;
    }

    public void Attack(Collider target)
    {
        if (target.gameObject.GetInstanceID() != _data.Parent_Ship_ID)
        {
            Bullet_Data.Bullet_Destroy(gameObject);
            if (target.gameObject.GetComponent<IShips>() != null)
            {
                target.gameObject.GetComponent<IShips>().TakeDamage(_data.Damage);
            }
            
        }
    }
}
