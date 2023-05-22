using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private GameStateManager gameManager;
    [Header("Menus")]
    public Transform pauseMenu;
    public Transform gameOverMenu;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        gameManager = GameStateManager.Instance;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        gameManager.GamePause();
    }
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);        
    }
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
