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

    /*public Animator sceneChanger;
    private static readonly int Dead = Animator.StringToHash("Death");*/
    
    protected override void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
        _cineMachine = FindObjectOfType<CinemachineVirtualCamera>();
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _levelManager.SetInitialRoom();
        _currentRoom = _levelManager.GetCurrentRoom();
        _playerSpawnPoint = _currentRoom.transform.Find("PlayerSpawn").gameObject;
        player = Instantiate(player,_playerSpawnPoint.transform.position,Quaternion.identity);
        _cineMachine.Follow = player.transform;

        AudioManager.Instance.PlaySoundTrack();
    }

    /*// Update is called once per frame
    private void Update()
    {
        if (PlayerEvents.PlayerDead != null)
        {
            Debug.Log("scene change");
            SceneChange();
        }
    }

    private void SceneChange()
    {
        sceneChanger.SetTrigger(Dead);
        StartCoroutine(LoadMenu());
    }

    private static IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }*/

    public void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, 0);
    }

    public static void BackToMainMenu()
    {
        AudioManager.Instance.StopSoundTrack();
        SceneManager.LoadScene("Menu");
    }
    
    public void LoadDeathScreen()
    {
        AudioManager.Instance.StopSoundTrack();
        animator.SetTrigger(Death);
        StartCoroutine(DeathScreen());
    }
    
    private static IEnumerator DeathScreen()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }
}
