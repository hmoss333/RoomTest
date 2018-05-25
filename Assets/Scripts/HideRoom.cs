using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    public List<Transform> adjactentRooms;

    // Use this for initialization
	void Start () {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }

        meshesEnabled = false;

        //FindAdjactentRooms();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && meshesEnabled == false)
        {
            TurnOnMesh();
            //TurnOnAdjactendRooms();

            meshesEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("left room");
            //renderer.enabled = true;

            TurnOffMesh();
            //TurnOffAdjactendRooms();

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

    //void FindAdjactentRooms()
    //{
    //    WaypointScript currentRoom = GetComponentInParent<WaypointScript>();

    //    adjactentRooms = currentRoom.waypointsInRange;
    //}

    //void TurnOnAdjactendRooms()
    //{
    //    foreach (Transform room in adjactentRooms)
    //    {
    //        room.GetComponentInChildren<HideRoom>().TurnOnMesh();
    //    }
    //}

    //void TurnOffAdjactendRooms()
    //{
    //    foreach (Transform room in adjactentRooms)
    //    {
    //        room.GetComponentInChildren<HideRoom>().TurnOffMesh();
    //    }
    //}
}
