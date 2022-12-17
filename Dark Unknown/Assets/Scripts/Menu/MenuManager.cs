using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public enum Menu
    {
        Main,
        Options,
        Audio,
        Graphics,
        Keybindings,
        Credits,
        Suggestions
    };
    
    public Animator animator;
    private static readonly int Quit = Animator.StringToHash("Quit");
    
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject AudioMenu;
    public GameObject GraphicsMenu;
    public GameObject KeybindingsMenu;
    public GameObject CreditsMenu;
    public GameObject SuggestionsMenu;

    public Slider _playerVolumeSlider;
    public Slider _enemyVolumeSlider;
    public Slider _backgroundVolumeSlider;

    private Resolution[] _resolutions;
    public Dropdown resolutionDropdown;

    private GameObject[] _keybindingButtons;
    public static bool IsChangingKey;
    [SerializeField] private Texture2D customCursor;


    //generic function to activate a certain menu screen
    private void SetMenu(Menu menu)
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        AudioMenu.SetActive(false);
        GraphicsMenu.SetActive(false);
        KeybindingsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        SuggestionsMenu.SetActive(false);

        switch (menu)
        {
            case Menu.Main:
                MainMenu.SetActive(true);
                break;
            case Menu.Options:
                OptionsMenu.SetActive(true);
                break;
            case Menu.Audio:
                AudioMenu.SetActive(true);
                break;
            case Menu.Graphics:
                GraphicsMenu.SetActive(true);
                break;
            case Menu.Keybindings:
                KeybindingsMenu.SetActive(true);
                break;
            case Menu.Credits:
                CreditsMenu.SetActive(true);
                break;
            case Menu.Suggestions:
                SuggestionsMenu.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        SetMenu(Menu.Main);

        Resources.Load("Keybindings");
        Resources.Load("SavedKeybindings");

        var resolutions = Screen.resolutions.Select(resolution => new Resolution
        {
            width = resolution.width, height = resolution.height
        }).Distinct();
        _resolutions = resolutions as Resolution[] ?? resolutions.ToArray();
        
        resolutionDropdown.ClearOptions();
        var options = new List<string>();
        var currentResolutionIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
    
            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        _playerVolumeSlider.value = AudioManager.Instance.GetPlayerVolumeSound();
        _enemyVolumeSlider.value = AudioManager.Instance.GetEnemyVolumeSound();
        _backgroundVolumeSlider.value = AudioManager.Instance.GetBackgroundVolumeSound();
    }

    private void Update()
    {
        if ((AudioMenu.activeSelf ||
            GraphicsMenu.activeSelf ||
            (KeybindingsMenu.activeSelf && !IsChangingKey)) &&
            Input.GetKeyDown(KeyCode.Escape))   //return to main menu
        {
            SetMenu(Menu.Options);
            KeybindManager.Instance.ResetIfNotSaved();
        }
        else if (KeybindingsMenu.activeSelf && IsChangingKey)
        {
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMenu(Menu.Main);
        }
    }
    
    //reactors to the pressing of a button
    public void OpenMainMenu()
    {
        SetMenu(Menu.Main);
    }
    
    public void OpenOptionsMenu()
    {
        SetMenu(Menu.Options);
    }
    
    public void OpenAudioMenu()
    {
        SetMenu(Menu.Audio);
    }
    
    public void OpenGraphicsMenu()
    {
        SetMenu(Menu.Graphics);
    }
    
    public void OpenKeybindingsMenu()
    {
        SetMenu(Menu.Keybindings);
    }

    public void OpenCreditsMenu()
    {
        SetMenu(Menu.Credits);
    }
    
    public void OpenSuggestionsMenu()
    {
        SetMenu(Menu.Suggestions);
    }
    
    public void Submit()
    {
        SetMenu(Menu.Main);
    }

    public void PlayGame()
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
        animator.SetTrigger(Quit);
        StartCoroutine(LoadGameCoroutine());
    }
    
    private static IEnumerator LoadGameCoroutine()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        animator.SetTrigger(Quit);
        Debug.Log("Quit game...");
        StartCoroutine(QuitCoroutine());
    }
    
    private static IEnumerator QuitCoroutine()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
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

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void UpdateKeyText(string key, KeyCode code)
    {
        var tmp = Array.Find(_keybindingButtons, x => x.name == key).GetComponentInChildren<Text>();
        tmp.text = code.ToString();
    }

}
