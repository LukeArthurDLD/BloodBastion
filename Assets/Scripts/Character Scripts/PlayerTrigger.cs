using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{

    private bool _rainPlay;
    public GameObject rain;
    public GameObject deathScreen;
    public GameObject victoryScreen;
    public GameObject waypointImage;
    public GameObject enemyNearImage;
    public AudioSource zombieNear;
    public AudioSource walkingAudio;
    public GameObject turnOffUI;
    public GameObject pressToWin;
    public GameObject playerGun;
    public GameObject playerManager;

    private void Start()
    {
        rain.SetActive(true);
    }

    void Update()
    {
        if (_rainPlay)
        {
            rain.SetActive(true);
        }
    }

    private void PlayerDeath()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        deathScreen.SetActive(true);
        waypointImage.SetActive(false);
        enemyNearImage.SetActive(false);
        zombieNear.Stop();
        rain.SetActive(false);
        walkingAudio.Stop();
        turnOffUI.SetActive(false);
        playerGun.SetActive(false);
        playerManager.SetActive(false);
    }

    private void PlayerVictory()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
        waypointImage.SetActive(false);
        enemyNearImage.SetActive(false);
        zombieNear.Stop();
        rain.SetActive(false);
        walkingAudio.Stop();
        playerManager.SetActive(false);
        turnOffUI.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Building")
        {
            _rainPlay = false;
        }
        else
            _rainPlay = true;
        if (other.gameObject.tag == "Enemy")
        {
            PlayerDeath();
        }
        if (other.gameObject.tag == "VictoryState")
        {
            pressToWin.SetActive(true);
            playerGun.SetActive(true);
            if (Input.GetKey("e"))
            {
                PlayerVictory();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VictoryState")
        {
            pressToWin.SetActive(false);
        }
    }

}
