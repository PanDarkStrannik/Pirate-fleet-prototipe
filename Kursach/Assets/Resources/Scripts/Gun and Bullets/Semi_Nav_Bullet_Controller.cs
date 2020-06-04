using UnityEngine;

public class Semi_Nav_Bullet_Controller : Simple_Bullet_Controller
{

    [Tooltip("Радиус обнаружения для пули")]
    [SerializeField] private float _sphere_radius=1f;

    protected override void FixedUpdate()
    {
        Movement();
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, _sphere_radius, out hit))
        {
            if (hit.collider.gameObject.GetComponent<IShips>() != null)
            {
               
                _bullet_body.Sleep();
                transform.LookAt(hit.transform.position);
                Movement();
            }

        }
        base.FixedUpdate();
    }

   
}
