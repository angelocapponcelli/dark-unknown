using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int PlayerHasWon = Animator.StringToHash("PlayerHasWon");
    private static readonly int Choice = Animator.StringToHash("Choice");

    private void Start()
    {
        Debug.Log(GameManager.PlayerHasWon);
        animator.SetBool(PlayerHasWon, GameManager.PlayerHasWon);
    }

    public void BackToMainMenu()
    {
        animator.SetTrigger(Choice);
        StartCoroutine(MainMenu());
    }
    
    private static IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
    
    public void RestartGame()
    {
        animator.SetTrigger(Choice);
        StartCoroutine(Restart());
    }
    
    private static IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        animator.SetTrigger(Choice);
        Debug.Log("Quit game...");
        StartCoroutine(Quit());
    }
    
    private static IEnumerator Quit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    //UI-AUDIO
    public void PlayOverUIButtonSound()
    {
        AudioManager.Instance.PlayOverUIButtonSound();
    }
    public void PlayClickUIButtonSound()
    {
        AudioManager.Instance.PlayClickUIButtonSound();
    }
}
