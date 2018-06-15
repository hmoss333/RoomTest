using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    public GameObject doorPrefab;
    public bool roomLocked = true;
    public string sigilWord;

    WaypointScript wayPoint;
    HideRoom roomScript;
    public LayerMask layerMask;

    public List<GameObject> connectedDoors = new List<GameObject>();
    
    // Use this for initialization
	void Start () {
        roomScript = GetComponent<HideRoom>();
        wayPoint = GetComponentInParent<WaypointScript>();
        doorPrefab = Resources.Load("Door") as GameObject;

        CreateDoors(wayPoint, doorPrefab);
	}
	
	// Update is called once per frame
	void Update () {
        if (roomLocked)
        {
            if (roomScript.meshesEnabled || roomScript.litByFlashlight)
                LockRoom(roomScript.roomMeshes);
        }
    }

    void LockRoom(MeshRenderer[] roomMeshes)
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }
    }

    void CreateDoors(WaypointScript currentRoom, GameObject prefab)
    {
        GameObject door = null;
        List<Transform> roomList = currentRoom.adjactentNodes;

        foreach(Transform room in roomList)
        {
            WaypointScript wayPoint = room.GetComponentInParent<WaypointScript>();
            HideRoom roomScript = room.GetComponentInChildren<HideRoom>();

            if (currentRoom.xPos == wayPoint.xPos && currentRoom.yPos == wayPoint.yPos + 1)
            {
                door = Instantiate(prefab, 
                    new Vector3(roomScript.transform.position.x, roomScript.transform.position.y, roomScript.transform.position.z + (WaypointManager.scale / 2)), 
                    Quaternion.Euler(0, 0, 0), 
                    roomScript.transform);
            }
            else if (currentRoom.xPos == wayPoint.xPos && currentRoom.yPos == wayPoint.yPos - 1)
            {
                door = Instantiate(prefab, 
                    new Vector3(roomScript.transform.position.x, roomScript.transform.position.y, roomScript.transform.position.z - (WaypointManager.scale / 2)), 
                    Quaternion.Euler(0, 0, 0), 
                    roomScript.transform);
            }
            else if (currentRoom.xPos == wayPoint.xPos + 1&& currentRoom.yPos == wayPoint.yPos)
            {
                door = Instantiate(prefab, 
                    new Vector3(roomScript.transform.position.x + (WaypointManager.scale / 2), roomScript.transform.position.y, roomScript.transform.position.z), 
                    Quaternion.Euler(0, 90, 0), 
                    roomScript.transform);
            }
            else if (currentRoom.xPos == wayPoint.xPos - 1 && currentRoom.yPos == wayPoint.yPos)
            {
                door = Instantiate(prefab, 
                    new Vector3(roomScript.transform.position.x - (WaypointManager.scale / 2), roomScript.transform.position.y, roomScript.transform.position.z), 
                    Quaternion.Euler(0, 90, 0), 
                    roomScript.transform);
            }

            door.transform.localScale = new Vector3(0.4f, 1f, 0.05f); //need to find a better way to set object scale before coming here. will not scale properly
            door.GetComponent<InteractDoor>().doorManager = this;
            door.GetComponent<InteractDoor>().sigilWord = sigilWord;
            roomScript.UpdateMeshes();

            connectedDoors.Add(door);
        }
    }

    public void DestroyDoors(List<GameObject> doorList)
    {
        foreach (GameObject door in doorList)
        {
            door.SetActive(false);
            //doorList.Remove(door);
        }
    }
}
