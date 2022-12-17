using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    public enum Menu
    {
        Main,
        Options,
        Credits,
        Suggestions
    };
    
    public Animator animator;
    private static readonly int Quit = Animator.StringToHash("Quit");
    
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject SuggestionsMenu;

    public Slider _playerVolumeSlider;
    public Slider _enemyVolumeSlider;
    public Slider _backgroundVolumeSlider;
    
    private CursorManager _cursorManager;


    //generic function to activate a certain menu screen
    private void SetMenu(Menu menu)
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
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

        _playerVolumeSlider.value = AudioManager.Instance.GetPlayerVolumeSound();
        _enemyVolumeSlider.value = AudioManager.Instance.GetEnemyVolumeSound();
        _backgroundVolumeSlider.value = AudioManager.Instance.GetBackgroundVolumeSound();
        _cursorManager = FindObjectOfType<CursorManager>();
        _cursorManager.setMenuCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //return to main menu
        {
            SetMenu(Menu.Main);
        }
    }
    
    //reactors to the pressing of a button
    public void OpenMainMenu()
    {
        _cursorManager.setMenuCursor();
        SetMenu(Menu.Main);
    }
    
    public void OpenOptionsMenu()
    {
        SetMenu(Menu.Options);
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
        animator.SetTrigger(Quit);
        _cursorManager.setPlayCursor();
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
}
