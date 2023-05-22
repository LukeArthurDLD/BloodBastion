using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    private LevelManager levelManager;
    public enum GameState { Paused, Playing, GameOver }
    public GameState state;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        levelManager = LevelManager.Instance;
        GameStart();
    }
    public void GameStart()
    {
        Time.timeScale = 1;
        state = GameState.Playing;

        if (levelManager != null && levelManager.gameOverMenu != null)
            levelManager.gameOverMenu.gameObject.SetActive(false);
    }
    public void GamePause()
    {
        Time.timeScale = 0;
        state = GameState.Paused;

    }
    public void GameOver()
    {
        Time.timeScale = 0;
        state = GameState.GameOver;
        if (levelManager != null && levelManager.gameOverMenu != null)
            levelManager.gameOverMenu.gameObject.SetActive(true);
    }
}
