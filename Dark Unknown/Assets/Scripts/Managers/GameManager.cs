using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelManager _levelManager;
    [SerializeField] private Player player;
    
    protected override void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _levelManager.SetInitialRoom();
        player = Instantiate(player,Vector3.zero,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 0, 0);
    }
}
