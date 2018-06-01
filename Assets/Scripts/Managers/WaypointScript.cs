using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour {

    public WaypointManager wayPointManager;  //The reference type is stored here , in this case the manager script.
    public Transform waypointInRange;       //This is the waypoint that is closest to this waypoint.
    
	public bool showDebugLine = true;       //By defualt this is true, but you make it false if you like, its for debugging.
    public Color debugGizmoColor = Color.red; //Same as above, just a color for the gizmo line.

    public enum Type { start, wall, corner, empty, eventRoom }
    public Type type;
    public enum Direction { top, bottom, left, right}
    public Direction direction;

	public int xPos;
	public int yPos;
    public int zPos;

    public GameObject roomPrefab;

    public List<Transform> adjactentNodes;


    //This method is called after the manager creates the list of waypoints. It just stores the manager script we passed and then
    //calls the GetWaypointDirection from the manager.
    public void StartLooking(WaypointManager managerScript)
    {
        wayPointManager = managerScript;
        waypointInRange = wayPointManager.GetWaypointDirection(this.transform);
        SelectRoomType(type);
        //roomPrefab = Instantiate(wayPointManager.roomTypes[0], new Vector3(transform.position.x, wayPointManager.roomTypes[0].transform.position.y, transform.position.z), Quaternion.identity);
        //roomPrefab.transform.parent = this.transform;

        adjactentNodes = GetAdjacentNodes();
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

    void SelectRoomType(Type roomType)
    {
        GameObject roomPrefab = null;
        GameObject[] roomTypeArray;

        switch (roomType)
        {
            case Type.start:
                roomTypeArray = wayPointManager.startRooms;
                roomPrefab = SelectRandomPrefab(roomTypeArray);
                break;
            case Type.empty:
                roomTypeArray = wayPointManager.emptyRoomTypes;
                roomPrefab = SelectRandomPrefab(roomTypeArray);
                break;
            case Type.eventRoom:
                roomTypeArray = wayPointManager.eventRoomTypes;
                roomPrefab = SelectRandomPrefab(roomTypeArray);
                break;
            case Type.wall:
                roomTypeArray = wayPointManager.wallRoomTypes;
                roomPrefab = SelectRandomPrefab(roomTypeArray);
                break;
            case Type.corner:
                roomTypeArray = wayPointManager.cornerRoomTypes;
                roomPrefab = SelectRandomPrefab(roomTypeArray);
                break;
            default:
                Debug.Log("Not a valid room type");
                break;
        }

        roomPrefab = Instantiate(roomPrefab, transform.position/*new Vector3(transform.position.x, roomPrefab.transform.position.y, transform.position.z)*/, Quaternion.identity);
        roomPrefab.transform.localScale = new Vector3(WaypointManager.scale, WaypointManager.scale, WaypointManager.scale);

        if (roomType == Type.wall || roomType == Type.corner || roomType == Type.start)
            RotateRoom(roomPrefab, direction);

        roomPrefab.transform.parent = this.transform;
    }

    GameObject SelectRandomPrefab (GameObject[] roomTypeArray)
    {
        int randNum = Random.Range(0, roomTypeArray.Length);

        return roomTypeArray[randNum];
    }

    void RotateRoom(GameObject roomPrefab, Direction roomDirection)
    {
        switch (roomDirection)
        {
            case Direction.top:
                roomPrefab.transform.Rotate(new Vector3(0, 180));
                break;
            case Direction.bottom:
                roomPrefab.transform.Rotate(new Vector3(0, 0));
                break;
            case Direction.left:
                roomPrefab.transform.Rotate(new Vector3(0, 90));
                break;
            case Direction.right:
                roomPrefab.transform.Rotate(new Vector3(0, 270));
                break;
            default:
                break;
        }
    }

    List<Transform> GetAdjacentNodes()
    {
        List<Transform> tempList = new List<Transform>();

        WaypointScript[] roomList;
        roomList = GameObject.FindObjectsOfType<WaypointScript>();

        foreach (WaypointScript room in roomList)
        {
            if (room != this && room.zPos == this.zPos)
            {
                //This will only include rooms immediately above/bellow/left/right of current room
                if ((room.xPos == this.xPos + 1 && room.yPos == this.yPos)
                    || (room.xPos == this.xPos - 1 && room.yPos == this.yPos)
                    || (room.yPos == this.yPos + 1 && room.xPos == this.xPos)
                    || (room.yPos == this.yPos - 1 && room.xPos == this.xPos))
                {
                    tempList.Add(room.transform);
                }
            }
        }

        return tempList;
    }
}
