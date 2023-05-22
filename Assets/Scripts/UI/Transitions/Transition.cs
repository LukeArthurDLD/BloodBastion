using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Transition : MonoBehaviour
{

    public Animator animator;
    //public ParticleSystem rain;
    // Start is called before the first frame update
    void Start()
    {
        animator.SetTrigger("FadeIn");
    }

    public void StartGame(int sceneIndex)
    {
        StartCoroutine(FadeInToStartGame(sceneIndex));
    }

    public void MainMenu()
    {
        StartCoroutine(FadeIntoMainMenu());
    }
    
    IEnumerator FadeIntoMainMenu()
    {
        Time.timeScale = 1.0f;
        //rain.Stop();
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator FadeInToStartGame(int sceneIndex)
    {
        Time.timeScale = 1f;
        //rain.Stop();
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2.2f);

        SceneManager.LoadScene(sceneIndex == 1 ? "Greybox" : "Level 2");
    }
}
