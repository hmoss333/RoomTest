using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREATE A EMPTY GAMEOBJECT AND ATTACH THIS SCRIPT TO IT. THEN ADD WAYPOINTS TO THIS GAMEOBJECT AS CHILDREN. 
//CHANGE THE WAYPOINTTOWAYPOINTRADIUS AS NEEDED SO THE WAYPOINTS CAN FIND ITS CLOSEST WAYPOINT.
public class WaypointManager : MonoBehaviour {

    public List<Transform> waypointNodes;         //Create a list of all the waypoints in scene( its like a array but more easy to work with)
    public int totalWaypoints = 0;                //Shows the total count of waypoints in the scene.
    public float waypointToWaypointRadius = 50f;  //a radius that each waypoint is checking for to see if another waypoint is close by

    public GameObject[] roomTypes;

    public GameObject player;

    void Awake()
    {
        //Start checking for the waypoints as children in gameobject and add them to the list. Also add the WayPointScript to each.
        foreach (Transform waypoint in GetComponentsInChildren<Transform>())
        {
            if (waypoint != this.transform)
            {
                waypointNodes.Add(waypoint);
                waypoint.gameObject.AddComponent<WaypointScript>();
            }
        }

        //Once the creation of the list above is complete then have each waypoint check for other waypoints. Also make a reference to this manager
        //on each waypoint.
        foreach (Transform waypoints in waypointNodes)
        {
            //We pass this script to each waypoint so we can interact with it.
            waypoints.GetComponent<WaypointScript>().StartLooking(this);
        }

        //Just gets the total count of Transforms in the waypoint list.
        totalWaypoints = waypointNodes.Count;

        player = Instantiate(player, new Vector3(waypointNodes[0].position.x, 1, waypointNodes[0].position.z), Quaternion.identity); //testing for now
    }

    //After the list is created this method is called by each waypoint and this script does all the leg work.
    //Add your checks for north, south, west, east, on here.
    public Transform GetWaypointDirection(Transform waypoint)
    {

        Transform wayPointFound = null;

        foreach (Transform wayPoint in waypointNodes)
        {
            //Check for the distance of each waypoint in the list and return the one closest to the transform that called this method.
            float distanceCheck = Vector3.Distance(waypoint.position, wayPoint.position);

            if (distanceCheck < waypointToWaypointRadius)
            {
                wayPointFound = wayPoint;
            }
        }
        return wayPointFound;
    }
}
