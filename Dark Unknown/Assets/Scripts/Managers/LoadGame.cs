using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PlayGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
}
