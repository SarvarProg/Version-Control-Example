using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); SceneManager.LoadScene(0);
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

}
