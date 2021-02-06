using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    public enum GameOverState
    {
        Win, Lose
    }

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameNamager instance is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadSceneAsync("Game");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        SpawnManager.Instance.StartSpawning();
        UIManager.Instance.ShowInstructions(false);
    }

    public void GameOver(GameOverState gameOverState = GameOverState.Lose)
    {
        _isGameOver = true;
        if (gameOverState == GameOverState.Win)
        {
            UIManager.Instance.GameWinSequence();
        } else
        {
            UIManager.Instance.GameLoseSequence();
        }
        
        SpawnManager.Instance.OnGameOver();
    }
}
