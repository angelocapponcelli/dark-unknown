using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private static readonly int Death = Animator.StringToHash("Death");
    public void BackToMainMenu()
    {
        animator.SetTrigger(Death);
        StartCoroutine(MainMenu());
    }
    
    private static IEnumerator MainMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
    
    public void RestartGame()
    {
        animator.SetTrigger(Death);
        StartCoroutine(Restart());
    }
    
    private static IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        animator.SetTrigger(Death);
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
