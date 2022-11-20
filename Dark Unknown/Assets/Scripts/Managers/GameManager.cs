using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private LevelManager _levelManager;
    [SerializeField] private GameObject _initialRoom;

    protected override void Awake()
    {
        base.Awake();
        _levelManager = FindObjectOfType<LevelManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _levelManager.SetInitialRoom(_initialRoom);
        //Instantiate(_initialRoom, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
