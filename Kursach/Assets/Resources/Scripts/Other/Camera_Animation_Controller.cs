using UnityEngine;

public class Camera_Animation_Controller : MonoBehaviour
{
  private static Animator _camera_animation;
    void Start()
    {
        _camera_animation = GetComponent<Animator>();
    }


    public static void Shoot_Animation()
    {
        _camera_animation.SetTrigger("Is_Shoot");
    }

    public static void Explouzion_Animation()
    {
        _camera_animation.SetTrigger("Is_Explouzion");
    }
}
