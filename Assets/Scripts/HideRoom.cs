using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

	public List<WaypointScript> adjactentRooms;

    // Use this for initialization
	void Start () {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }

        meshesEnabled = false;

		GetAdjacentRooms();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
		if (other.tag == "Player")// && meshesEnabled == false)
        {
            TurnOnMesh();
			TurnOnAdjacentRooms (adjactentRooms);

            meshesEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TurnOffMesh();
			TurnOffAdjacentRooms (adjactentRooms);

            meshesEnabled = false;
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
		WaypointScript thisNode = GetComponentInParent<WaypointScript> ();

		WaypointScript[] roomList;
		roomList = GameObject.FindObjectsOfType<WaypointScript> ();

		foreach (WaypointScript room in roomList) 
		{
			if (room != thisNode) 
			{	
				if (room.xPos == thisNode.xPos || room.xPos == thisNode.xPos + 1 || room.xPos == thisNode.xPos - 1) 
				{
					if (room.yPos == thisNode.yPos || room.yPos == thisNode.yPos + 1 || room.yPos == thisNode.yPos - 1) 
					{
						adjactentRooms.Add (room);
					}
				}
			}
		}
	}

	public void TurnOnAdjacentRooms(List<WaypointScript> wayPoints) 
	{
		foreach (WaypointScript node in wayPoints) 
		{
			node.GetComponentInChildren<HideRoom> ().TurnOnMesh ();
		}
	}

	public void TurnOffAdjacentRooms(List<WaypointScript> wayPoints) 
	{
		foreach (WaypointScript node in wayPoints) 
		{
			node.GetComponentInChildren<HideRoom> ().TurnOffMesh ();
		}
	}
}
