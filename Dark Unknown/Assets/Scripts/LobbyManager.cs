using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_gameManager.getLobby());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
