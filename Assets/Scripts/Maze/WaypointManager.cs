using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CREATE A EMPTY GAMEOBJECT AND ATTACH THIS SCRIPT TO IT. THEN ADD WAYPOINTS TO THIS GAMEOBJECT AS CHILDREN. 
//CHANGE THE WAYPOINTTOWAYPOINTRADIUS AS NEEDED SO THE WAYPOINTS CAN FIND ITS CLOSEST WAYPOINT.
public class WaypointManager : MonoBehaviour
{

    [Header("Map Settings")]
    public int xMax;
    public int yMax;
    public static float scale = 1;
    public float scaleNum;

    [Header("Waypoint Controls")]
    public List<Transform> waypointNodes;         //Create a list of all the waypoints in scene( its like a array but more easy to work with)
    public List<Transform> wallNodes;
    public int totalWaypoints = 0;                //Shows the total count of waypoints in the scene.
    public float waypointToWaypointRadius = 50f;  //a radius that each waypoint is checking for to see if another waypoint is close by
    [Range(0f, 1f)]
    public float eventRoomProbability;

    [Header("Prefabs")]
    public GameObject node;
    public GameObject player;
    public GameObject killer;
    //public GameObject[] roomTypes;

    public static bool spawnedStart;
    public GameObject[] startRooms;
    public GameObject[] cornerRoomTypes;
    public GameObject[] wallRoomTypes;
    public GameObject[] emptyRoomTypes;
    public GameObject[] eventRoomTypes;

    GameManager gm;

    void Awake()
    {
        scale = scaleNum;
        spawnedStart = false;

        GenerateNodes(node, xMax, yMax, scale);

        //Start checking for the waypoints as children in gameobject and add them to the list. Also add the WayPointScript to each.
        foreach (Transform waypoint in GetComponentsInChildren<Transform>())
        {
            if (waypoint != this.transform)
            {
                waypointNodes.Add(waypoint);
                //waypoint.gameObject.AddComponent<WaypointScript>();
            }
        }

        //Check available walls to generate a starting node
        GenerateStartNode(Random.Range(0, wallNodes.Count), wallNodes);

        //Once the creation of the list above is complete then have each waypoint check for other waypoints. Also make a reference to this manager
        //on each waypoint.
        foreach (Transform waypoints in waypointNodes)
        {
            //We pass this script to each waypoint so we can interact with it.
            waypoints.GetComponent<WaypointScript>().StartLooking(this);
        }

        //Just gets the total count of Transforms in the waypoint list.
        totalWaypoints = waypointNodes.Count;

        GameManager.UpdateStep();

        ////Get the position of the starting node for instantiating players; can probably do this as a public variable so other scripts can reference, i.e. GameManager
        //Vector3 startPos = GetStartNode(waypointNodes).position;
        //player = Instantiate(player, new Vector3(startPos.x, 1 - (scale / 4), startPos.z), Quaternion.identity);//testing for now; need to move to GameManager
        ////player.transform.localScale = new Vector3(scale / 4, scale / 4, scale / 4);

        //int randNum = Random.Range(0, waypointNodes.Count);
        //killer = Instantiate(killer, new Vector3(waypointNodes[randNum].position.x, 1 - (scale / 4), waypointNodes[randNum].position.z), Quaternion.identity); //testing for now; need to move to GameManager
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

    void GenerateNodes(GameObject node, int xNum, int yNum, float scaleNum)
    {
        for (int i = 0; i < xNum; i++)
        {
            for (int j = 0; j < yNum; j++)
            {
                GameObject newnode = Instantiate(node, new Vector3(i * scaleNum, 0, j * scaleNum), Quaternion.identity);
                newnode.transform.parent = GameObject.FindObjectOfType<WaypointManager>().transform;
                WaypointScript wayPointRef = newnode.AddComponent<WaypointScript>();
                wayPointRef.xPos = i;
                wayPointRef.yPos = j;

                if (i == 0 || i == xNum - 1)
                {
                    if (j == 0 || j == yNum - 1)
                    {
                        wayPointRef.type = WaypointScript.Type.corner;
                        if (i == 0)
                        {
                            if (j == 0)
                                wayPointRef.direction = WaypointScript.Direction.bottom;
                            else
                                wayPointRef.direction = WaypointScript.Direction.left;
                        }
                        else
                        {
                            if (j == 0)
                                wayPointRef.direction = WaypointScript.Direction.right;
                            else
                                wayPointRef.direction = WaypointScript.Direction.top;
                        }
                    }
                    else
                    {
                        wayPointRef.type = WaypointScript.Type.wall;
                        if (i == 0)
                            wayPointRef.direction = WaypointScript.Direction.left;
                        else
                            wayPointRef.direction = WaypointScript.Direction.right;
                        wallNodes.Add(newnode.transform);
                    }
                }
                else
                {
                    if (j == 0 || j == yNum - 1)
                    {
                        wayPointRef.type = WaypointScript.Type.wall;
                        if (j == 0)
                            wayPointRef.direction = WaypointScript.Direction.bottom;
                        else
                            wayPointRef.direction = WaypointScript.Direction.top;
                        wallNodes.Add(newnode.transform);
                    }
                    else
                    {
                        if (Random.value < eventRoomProbability)
                            wayPointRef.type = WaypointScript.Type.eventRoom;
                        else
                            wayPointRef.type = WaypointScript.Type.empty;
                    }
                }
            }
        }
    }

    void GenerateStartNode(int nodeValue, List<Transform> nodeList)
    {
        WaypointScript room = nodeList[nodeValue].GetComponent<WaypointScript>();

        if (room.type == WaypointScript.Type.wall)
            nodeList[nodeValue].GetComponent<WaypointScript>().type = WaypointScript.Type.start;
        else
            GenerateStartNode(Random.Range(0, totalWaypoints), nodeList);
    }

    Transform GetStartNode(List<Transform> listOfNodes)
    {
        foreach (Transform currentNode in listOfNodes)
        {
            WaypointScript nodeData = currentNode.GetComponentInChildren<WaypointScript>();

            if (nodeData.type == WaypointScript.Type.start)
            {
                return nodeData.transform;
            }
        }

        return null;
    }
}
