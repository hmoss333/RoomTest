using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    public GameObject doorPrefab;
    public bool roomLocked = true;

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
        if ((roomScript.meshesEnabled || roomScript.litByFlashlight) && roomLocked == true)
        {
            LockRoom(roomScript.roomMeshes);
        }

        if (!roomLocked && connectedDoors.Count > 0)
        {
            DestroyDoors(connectedDoors);
        }
    }

    void LockRoom(MeshRenderer[] roomMeshes)
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }
    }

    //void ShowRoomObjects(MeshRenderer[] roomMeshes)
    //{
    //    foreach (MeshRenderer mesh in roomMeshes)
    //    {
    //        if (mesh.gameObject.tag != "Door")
    //        {
    //            mesh.enabled = true;
    //        }
    //    }
    //}

    void CreateDoors(WaypointScript currentRoom, GameObject prefab)
    {
        GameObject door = new GameObject();
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
            roomScript.UpdateMeshes();

            connectedDoors.Add(door);
        }
    }

    void DestroyDoors(List<GameObject> doorList)
    {
        foreach (GameObject door in doorList)
        {
            door.SetActive(false);
            doorList.Remove(door);
        }
    }
}
