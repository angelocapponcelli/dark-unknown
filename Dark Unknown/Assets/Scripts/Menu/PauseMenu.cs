using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private enum Menu
    {
        Main,
        Options
    };
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private Slider _playerVolumeSlider;
    [SerializeField] private Slider _enemyVolumeSlider;
    [SerializeField] private Slider _backgroundVolumeSlider;
    [SerializeField] private Texture2D customCursor;

    private void Start()
    {
        _playerVolumeSlider.value = AudioManager.Instance.GetPlayerVolumeSound();
        _enemyVolumeSlider.value = AudioManager.Instance.GetEnemyVolumeSound();
        _backgroundVolumeSlider.value = AudioManager.Instance.GetBackgroundVolumeSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        OpenMainMenu();
    }

    //generic function to activate a certain menu screen
    private void SetMenu(Menu menu)
    {
        _mainMenu.SetActive(false);
        _optionsMenu.SetActive(false);

        switch (menu)
        {
            case Menu.Main:
                _mainMenu.SetActive(true);
                break;
            case Menu.Options:
                _optionsMenu.SetActive(true);
                break;
        }
    }

    public void OpenMainMenu()
    {
        SetMenu(Menu.Main);
    }

    public void OpenOptionsMenu()
    {
        SetMenu(Menu.Options);
    }

    public void LoadMenu()
    {
        Resume(); //without this, the scene change in GameManager doesn't work
        GameManager.Instance.BackToMainMenu();
    }
    
    public void QuitGame()
    {
        Resume(); //without this, the scene change in GameManager doesn't work
        GameManager.Instance.QuitGame();
    }

    //UIAUDIO
    public void PlayOverUIButtonSound()
    {
        AudioManager.Instance.PlayOverUIButtonSound();
    }
    public void PlayClickUIButtonSound()
    {
        AudioManager.Instance.PlayClickUIButtonSound();
    }

    //OPTION SETTINGS
    public void SetBackgroundVolume(float value)
    {
        AudioManager.Instance.SetBackgroundVolume(value);
    }
    public void SetPlayerVolume(float value)
    {
        AudioManager.Instance.SetPlayerVolume(value);
    }
    public void SetEnemyVolume(float value)
    {
        AudioManager.Instance.SetSkeletonVolume(value);
    }
}
