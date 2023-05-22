using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCameraReverser : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public void RotatingCamera()
    {
        StartCoroutine(RotatorCourotine());
    }

    IEnumerator RotatorCourotine()
    {
        optionsMenu.SetActive(false);
        yield return new WaitForSeconds(1);
        mainMenu.SetActive(true);
    }
}
