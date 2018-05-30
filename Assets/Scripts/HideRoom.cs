using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    public bool litByFlashlight;

	public List<WaypointScript> adjactentRooms;

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            meshesEnabled = false;

            TurnOffMesh();
			//TurnOffAdjacentRooms (adjactentRooms);
        }
    }

    public void TurnOnMesh()
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = true;
        }
    }

    public void TurnOffMesh()
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
            if (room != thisNode)
            {
                //This will only include rooms immediately above/bellow/left/right of current room
                if ((room.xPos == thisNode.xPos + 1 && room.yPos == thisNode.yPos)
                    || (room.xPos == thisNode.xPos - 1 && room.yPos == thisNode.yPos)
                    || (room.yPos == thisNode.yPos + 1 && room.xPos == thisNode.xPos)
                    || (room.yPos == thisNode.yPos - 1 && room.xPos == thisNode.xPos))
                {
                    adjactentRooms.Add(room);
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

    //Unused
    public void TurnOnAdjacentRooms(List<WaypointScript> wayPoints) 
	{
		foreach (WaypointScript node in wayPoints) 
		{
			if (node.GetComponentInChildren<HideRoom> ().meshesEnabled == false) {
				node.GetComponentInChildren<HideRoom> ().TurnOnMesh ();
				node.GetComponentInChildren<HideRoom> ().meshesEnabled = true;
			}
		}
	}

	public void TurnOffAdjacentRooms(List<WaypointScript> wayPoints) 
	{
		foreach (WaypointScript node in wayPoints) 
		{
			node.GetComponentInChildren<HideRoom> ().TurnOffMesh ();
			node.GetComponentInChildren<HideRoom> ().meshesEnabled = false;
		}
	}
}
