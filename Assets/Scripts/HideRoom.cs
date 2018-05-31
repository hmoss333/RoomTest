using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    public bool litByFlashlight;

	public List<Transform> adjactentRooms;

    // Use this for initialization
	void Start () {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();
        litByFlashlight = false;

        foreach (MeshRenderer mesh in roomMeshes)
            mesh.enabled = false;

        meshesEnabled = false;
		GetAdjacentRooms();
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.flashlightOn && litByFlashlight)
        {
            TurnOnMesh();
        }
        else if (!meshesEnabled)
        {
            litByFlashlight = false;
            TurnOffMesh();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Player" && !meshesEnabled)
            {
                TurnOnMesh();
                //TurnOnAdjacentRooms (adjactentRooms);

                meshesEnabled = true;
        }

        //Temporary solution, should probably have something to call in Gregg instead of setting directly
        //Really need to update the logic better so that its not dependant on the room, but positioning
        if (other.tag == "Killer")
        {
            Gregg killer = other.GetComponent<Gregg>();
            killer.currentRoom = this.transform;
            killer.adjacentRooms = killer.GetAdjacentRooms(this.transform);
            killer.nextRoom = killer.SelectNextRoom(killer.adjacentRooms);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            meshesEnabled = false;

            TurnOffMesh();
        }
    }

    void TurnOnMesh()
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = true;
        }
    }

    void TurnOffMesh()
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }
    }

    void GetAdjacentRooms()
    {
        WaypointScript thisNode = GetComponentInParent<WaypointScript>();

        WaypointScript[] roomList;
        roomList = GameObject.FindObjectsOfType<WaypointScript>();

        foreach (WaypointScript room in roomList)
        {
            if (room != thisNode && room.zPos == thisNode.zPos)
            {
                //This will only include rooms immediately above/bellow/left/right of current room
                if ((room.xPos == thisNode.xPos + 1 && room.yPos == thisNode.yPos)
                    || (room.xPos == thisNode.xPos - 1 && room.yPos == thisNode.yPos)
                    || (room.yPos == thisNode.yPos + 1 && room.xPos == thisNode.xPos)
                    || (room.yPos == thisNode.yPos - 1 && room.xPos == thisNode.xPos))
                {
                    adjactentRooms.Add(room.transform);
                }

                //				//This will include diagonally alligned rooms
                //				if (room.xPos == thisNode.xPos || room.xPos == thisNode.xPos + 1 || room.xPos == thisNode.xPos - 1) 
                //				{
                //					if (room.yPos == thisNode.yPos || room.yPos == thisNode.yPos + 1 || room.yPos == thisNode.yPos - 1) 
                //					{
                //						adjactentRooms.Add (room);
                //					}
                //				}
            }
        }
    }

    public void UpdateMeshes()
    {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }
    }
}
