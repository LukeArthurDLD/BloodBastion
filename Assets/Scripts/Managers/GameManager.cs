using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Animator fadeInAnimator;
    public Animator fadeOutAnimator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TransitionCourotine());
    }

    
    IEnumerator TransitionCourotine()
    {
        fadeInAnimator.enabled = true;
        yield return new WaitForSeconds(1.05f);
        fadeInAnimator.enabled = false;
    }
}
