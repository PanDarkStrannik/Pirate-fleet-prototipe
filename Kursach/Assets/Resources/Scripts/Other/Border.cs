using UnityEngine;


/// <summary>
/// Класс содержщий информацию о границах экрана
/// </summary>
public class Border
{
    /// <summary>
    /// Ширина экрана
    /// </summary>
    private static float _borderWidth = 0;

    /// <summary>
    /// Получение ширины экрана
    /// </summary>
    public static float BorderWidth
    {
        get
        {
            if (_borderWidth == 0)
            {
                Camera main_camera = Camera.main;

                _borderWidth = main_camera.aspect * main_camera.orthographicSize;
            }
           
            return _borderWidth;
        }
    }

    /// <summary>
    /// Высота экрана (в случае, если ширина больше высоты)
    /// </summary>
    private static float _borderHeigth = 0;

    /// <summary>
    /// Получение высоты экрана (случай, когда ширина больше высоты)
    /// </summary>
    public static float BorderHeigth
    {
        get
        {
            if (_borderHeigth == 0)
            {
                Camera main_camera = Camera.main;

                _borderHeigth = main_camera.aspect * main_camera.orthographicSize / 2f;
             
            }
        
            return _borderHeigth;
        }
    }
}

