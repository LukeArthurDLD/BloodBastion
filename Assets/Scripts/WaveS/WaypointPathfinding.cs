using UnityEngine;

public class WaypointPathfinding : MonoBehaviour
{
    Enemy enemy;
    private Transform target;

    public WaypointManager manager;
    private int waypointIndex = 0;

    public bool doTurn;
    public float turnSpeed = 7f;

    [System.NonSerialized]
    public bool isPathFinished = false;
       
    public void Start()
    {
        enemy = GetComponent<Enemy>();
        target = manager.waypoints[0];
    }
    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * enemy.speed * Time.deltaTime, Space.World); //move toward waypoint

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation;
        rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;

        if(doTurn)
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        else
            transform.rotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f) //if reaches waypoint destination
        {
            GetNextWaypoint();
        }
    }
    void GetNextWaypoint()
    {
        if(waypointIndex >= manager.waypoints.Length - 1) //if last waypoint
        {
            if (!isPathFinished)
                isPathFinished = true;
            return;
        }
        //update target to next waypoint
        waypointIndex++;
        target = manager.waypoints[waypointIndex];
    }
   
}
