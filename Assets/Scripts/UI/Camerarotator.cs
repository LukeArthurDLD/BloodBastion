using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerarotator : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public void RotatingCamera()
    {
        StartCoroutine(RotatorCourotine());
    }

    IEnumerator RotatorCourotine()
    {
        mainMenu.SetActive(false);
        yield return new WaitForSeconds(1);
        optionsMenu.SetActive(true);
    }
}
