using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : Singleton<MenuManager>
{
    public enum Menu
    {
        Main,
        Options,
        Credits,
        Suggestions
    };

    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject SuggestionsMenu;
    
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
        SceneManager.LoadScene("Game");
        Destroy(gameObject);
    }

    public void QuitGame()
    {
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
