using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public Animator transitionAnimator;

    public void Play ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void StartGame()
    {
        StartCoroutine(FadeInToStartGame());
    }

    IEnumerator FadeInToStartGame()
    {
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2.2f);

        SceneManager.LoadScene("Greybox");
    }

}
