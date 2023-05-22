using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    //public Animator transitionAnimator;
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    public void TimeScaleOne()
    {
        Time.timeScale = 1f;
    }

    public void TimeScaleZero()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    //public void Home(int sceneIndex)
    //{
    //    StartCoroutine(FadeInToMainMenu());
    //}

    //IEnumerator FadeInToMainMenu()
    //{
    //    Time.timeScale = 1f;
    //    transitionAnimator.SetTrigger("FadeOut");
    //    yield return new WaitForSeconds(2.2f);

    //    SceneManager.LoadScene("MainMenu");
    //}
}
