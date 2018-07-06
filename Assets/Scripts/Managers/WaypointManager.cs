using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{

    [Header("Map Settings")]
    public int xMax;
    public int yMax;
    public int levels;
    public static float scale = 1;
    public float scaleNum;

    [Header("Waypoint Controls")]
    public List<Transform> waypointNodes; //Create a list of all the waypoints in scene( its like a array but more easy to work with)
    public List<Transform> wallNodes;
    List<Transform> stairNodes;
    public int totalWaypoints = 0; //Shows the total count of waypoints in the scene.
    float waypointToWaypointRadius = 1.5f * scale;  //a radius that each waypoint is checking for to see if another waypoint is close by
    [Range(0f, 1f)]
    public float eventRoomProbability;

    [Header("Prefabs")]
    public GameObject node;

    public static bool spawnedStart;
    public GameObject[] startRooms;
    public GameObject[] cornerRoomTypes;
    public GameObject[] wallRoomTypes;
    public GameObject[] emptyRoomTypes;
    public GameObject[] eventRoomTypes;
    public GameObject[] stairRoomTypes;
    public GameObject basementRoom;

    GameManager gm;

    void Awake()
    {
        if (levels < 1)
            levels = 1;
        scale = scaleNum;
        spawnedStart = false;

        GenerateNodes(node, xMax, yMax, levels, scale);

        //Start checking for the waypoints as children in gameobject and add them to the list. Also add the WayPointScript to each.
        foreach (Transform waypoint in GetComponentsInChildren<Transform>())
        {
            if (waypoint != this.transform)
            {
                waypointNodes.Add(waypoint);
            }
        }

        //Check available walls to generate a starting node
        GenerateStartNode(Random.Range(0, wallNodes.Count), wallNodes);
        basementRoom = Instantiate(basementRoom, new Vector3(0, -scale, 0), Quaternion.identity, this.transform);

        //If there are multiple levels to a map, generate entrance and exit staircase nodes
        if (levels > 1)
            GenerateStairNode(levels, waypointNodes);

        //Once the creation of the list above is complete then have each waypoint check for other waypoints. Also make a reference to this manager
        //on each waypoint.
        foreach (Transform waypoints in waypointNodes)
        {
            //We pass this script to each waypoint so we can interact with it.
            waypoints.GetComponent<WaypointScript>().StartLooking(this);
        }

        //Just gets the total count of Transforms in the waypoint list.
        totalWaypoints = waypointNodes.Count;

        //Start the game
        GameManager.UpdateStep();
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

    void GenerateNodes(GameObject node, int xNum, int yNum, int levelNum, float scaleNum)
    {
        for (int z = 0; z < levelNum; z++)
        {
            for (int i = 0; i < xNum; i++)
            {
                for (int j = 0; j < yNum; j++)
                {
                    GameObject newnode = Instantiate(node, new Vector3(i * scaleNum, z * scaleNum, j * scaleNum), Quaternion.identity);
                    newnode.transform.parent = GameObject.FindObjectOfType<WaypointManager>().transform;
                    WaypointScript wayPointRef = newnode.AddComponent<WaypointScript>();
                    wayPointRef.xPos = i;
                    wayPointRef.yPos = j;
                    wayPointRef.zPos = z;

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
    }

    void GenerateStartNode(int nodeValue, List<Transform> nodeList)
    {
        WaypointScript room = nodeList[nodeValue].GetComponent<WaypointScript>();

        if (room.type == WaypointScript.Type.wall && room.zPos == 0)
            nodeList[nodeValue].GetComponent<WaypointScript>().type = WaypointScript.Type.start;
        else
            GenerateStartNode(Random.Range(0, totalWaypoints), nodeList);
    }

    //TO DO: Update this function so that the position is randomized and then repeated on only the next floor; currenly only works for single position and only for 2 floors
    void GenerateStairNode(int floors, List<Transform> nodeList)
    {
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(nodeList);

        for (int i = floors; i > 0; i--)
        {
            int randNum = Random.Range(0, tempList.Count);
            WaypointScript room = tempList[randNum].GetComponent<WaypointScript>();

            if (room.zPos == i - 1)
            {
                if ((room.type == WaypointScript.Type.empty || room.type == WaypointScript.Type.eventRoom))
                {
                    room.type = WaypointScript.Type.stairs;
                }
                else
                {
                    //tempList.Remove(room.transform);
                    GenerateStairNode(i, tempList);
                    break;
                }
            }
            else
            {
                GenerateStairNode(i, tempList);
                break;
            }
        }
    }
}
