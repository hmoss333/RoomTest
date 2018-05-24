using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour {

    public WaypointManager wayPointManager;  //The reference type is stored here , in this case the manager script.
    public Transform waypointInRange;       //This is the waypoint that is closest to this waypoint.
    public bool showDebugLine = true;       //By defualt this is true, but you make it false if you like, its for debugging.
    public Color debugGizmoColor = Color.red; //Same as above, just a color for the gizmo line.

    public GameObject roomPrefab;


    //This method is called after the manager creates the list of waypoints. It just stores the manager script we passed and then
    //calls the GetWaypointDirection from the manager.
    public void StartLooking(WaypointManager managerScript)
    {
        wayPointManager = managerScript;
        waypointInRange = wayPointManager.GetWaypointDirection(this.transform);
        roomPrefab = Instantiate(wayPointManager.roomTypes[0], new Vector3(transform.position.x, wayPointManager.roomTypes[0].transform.position.y, transform.position.z), Quaternion.identity);
        roomPrefab.transform.parent = this.transform;
    }

    //This is not needed in your final build, its only used here to get an idea what waypoint connects to what.
    void OnDrawGizmosSelected()
    {
        if (showDebugLine)
        {
            if (waypointInRange != null)
            {
                Gizmos.color = debugGizmoColor;
                Gizmos.DrawLine(transform.position, waypointInRange.position);
                //Gizmos.color = Color.yellow;
                //Gizmos.DrawWireSphere(transform.position, wayPointManager.waypointToWaypointRadius);

            }

        }
    }
}
