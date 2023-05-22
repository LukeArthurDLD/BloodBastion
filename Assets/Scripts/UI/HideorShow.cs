using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideorShow : MonoBehaviour
{
    public GameObject arrowupordown;

    public void whenButtonClicked()
    {
        if (arrowupordown.activeInHierarchy == true)
            arrowupordown.SetActive(false);
        else
            arrowupordown.SetActive(true);
    }
}
