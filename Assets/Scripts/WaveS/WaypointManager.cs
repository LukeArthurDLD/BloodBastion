using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    //waypoint array
    public Transform[] waypoints;

    private void Awake()
    {
        waypoints = new Transform[transform.childCount]; //creates array the size of the child count
        for(int i = 0; i < waypoints.Length; i++) 
        {
            waypoints[i] = transform.GetChild(i); //binds each child transform to an array member
        }
    }
}
