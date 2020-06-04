using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_UI : MonoBehaviour
{
    [Tooltip("Меню паузы")]
    [SerializeField] private GameObject menu;
    [Tooltip("Описание меню паузы")]
    [SerializeField] private Text game_menu_description;
    [Tooltip("Кнопка продолжить")]
    [SerializeField] private Button resume_button;
    [Tooltip("Патроны игрока")]
    [SerializeField] private Text bullet_view;
    [Tooltip("Здоровье игрока")]
    [SerializeField] private Text health_view;


    private Player_Data _player;
    public Player_Data Player
    {
        set
        {
            _player = value;
        }
    }

    private float _max_health;
    private float _max_bullets;

    private bool _is_paused=false;

    private void Start()
    {
        game_menu_description.color = Color.white;
        game_menu_description.text = "Пауза";
        menu.SetActive(false);

        _max_health = _player.Health;
        _max_bullets = _player.BulletValue;
        _View_Player_Health();
        
    }

 
    private void Update()
    {
        _View_Player_Health();
        _View_Player_Bullet();

        if (Input.GetKeyDown(KeyCode.Escape) && !_is_paused)
        {
            _is_paused = true;
            _Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _is_paused && _player.Health > 0)
        {
            Resume();
            _is_paused = false;
        }
    }

    /// <summary>
    /// Отображение здоровья UI
    /// </summary>
    private void _View_Player_Health()
    {
        if (_player.Health >= _max_health*2/3)
        {
            health_view.color = Color.green;
        }
        else if (_player.Health < _max_health * 2 / 3 && _player.Health > _max_health / 3)
        {
            health_view.color = Color.yellow;
        }
        else if (_player.Health <= _max_health / 3)
        {
            health_view.color = Color.red;
        }
      
        if (_player.Health == 0)
        {
            resume_button.interactable = false;
            game_menu_description.color = Color.red;
            game_menu_description.text = "Вас взорвали(";
            Invoke("_Pause",2f);

        }

        health_view.text = $"Здоровье: {_player.Health}";
    }

    /// <summary>
    /// Отображение здоровья UI
    /// </summary>
    private void _View_Player_Bullet()
    {
        if (_player.BulletValue >= _max_bullets * 2 / 3)
        {
            bullet_view.color = Color.green;
        }
        else if (_player.BulletValue < _max_bullets * 2 / 3 && _player.BulletValue > _max_bullets / 3)
        {
            bullet_view.color = Color.yellow;
        }
        else if (_player.BulletValue <= _max_bullets / 3)
        {
            bullet_view.color = Color.red;
        }

        

        bullet_view.text = $"Боеприпасы: {_player.BulletValue}";
    }


    public void Win()
    {
        resume_button.interactable = false;
        game_menu_description.color = Color.cyan;
        game_menu_description.text = "Вы победили!";
        Invoke("_Pause", 2f);
    }

    /// <summary>
    /// Ставит сцену на паузу
    /// </summary>
    private void _Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }


    /// <summary>
    /// Снимает сцену с паузы
    /// </summary>
    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Перезагружает сцену, должна висеть на кнопке
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Выходит из игры, должна висеть на кнопке
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    

}
