using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionWaypoint : MonoBehaviour
{
    public Image waypointImage;
    public Transform waypointTarget;
    public TextMeshProUGUI meter;
    public Vector3 offset;
    void Update()
    {
        float minX = waypointImage.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = waypointImage.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.width - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(waypointTarget.position + offset);

        if (Vector3.Dot((waypointTarget.position), transform.forward) < 0)
        {
            // Target is behind Player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        waypointImage.transform.position = pos;
        meter.text = ((int)Vector3.Distance(waypointTarget.position, transform.position)).ToString() + "m";
    }
}
