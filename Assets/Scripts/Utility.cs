using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utility
{
    public static bool MouseOverUI()
    {
#if UNITY_STANDALONE 
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(0);
#endif
    }
}
