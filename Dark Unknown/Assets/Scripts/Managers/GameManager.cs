using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private LevelManager _levelManager;
    [SerializeField] private Player player;
    private CinemachineVirtualCamera _cineMachine;
    private RoomLogic _currentRoom;
    private GameObject _playerSpawnPoint;
    [SerializeField] private Animator animator;
    private static readonly int Death = Animator.StringToHash("Death");
    public static bool PlayerHasWon;
    private CursorManager _cursorManager;

    protected override void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
        _cineMachine = FindObjectOfType<CinemachineVirtualCamera>();
        _cursorManager = FindObjectOfType<CursorManager>();
        _cursorManager.setPlayCursor();
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _levelManager.SetInitialRoom();
        _currentRoom = _levelManager.GetCurrentRoom();
        _playerSpawnPoint = _currentRoom.transform.Find("PlayerSpawn").gameObject;
        player = Instantiate(player,_playerSpawnPoint.transform.position,Quaternion.identity);
        _cineMachine.Follow = player.transform;
        PlayerHasWon = false;

        AudioManager.Instance.PlaySoundTrack();
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, 0);
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance.StopSoundTrack();
        animator.SetTrigger(Death);
        _cursorManager.setMenuCursor();
        StartCoroutine(LoadMainMenu());
    }
    
    private static IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }
    
    public void LoadVictoryScreen()
    {
        PlayerHasWon = true;
        AudioManager.Instance.StopSoundTrack();
        UIController.Instance.DeactivatePlayerUI();
        animator.SetTrigger(Death);
        StartCoroutine(GameOverScreen());
    }
    
    public void LoadDeathScreen()
    {
        AudioManager.Instance.StopSoundTrack();
        UIController.Instance.DeactivatePlayerUI();
        animator.SetTrigger(Death);
        StartCoroutine(GameOverScreen());
    }
    
    private static IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }
    
    public void QuitGame()
    {
        AudioManager.Instance.StopSoundTrack();
        animator.SetTrigger(Death);
        Debug.Log("Quit game...");
        StartCoroutine(Quit());
    }
    
    private static IEnumerator Quit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
