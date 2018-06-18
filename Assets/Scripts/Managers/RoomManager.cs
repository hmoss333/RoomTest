using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    public bool litByFlashlight;
    bool roomLit = false;
    Color col;

    // Use this for initialization
    void Start()
    {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();
        litByFlashlight = false;

        foreach (MeshRenderer mesh in roomMeshes)
            mesh.enabled = false;

        meshesEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (meshesEnabled || (Player.flashlightOn && litByFlashlight))
        {
            TurnOnMesh();
        }
        else //if (!meshesEnabled)
        {
            TurnOffMesh();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !meshesEnabled)
        {
            TurnOnMesh();
            meshesEnabled = true;
            other.GetComponent<Player>().currentRoom = this.transform;
        }

        //Temporary solution, should probably have something to call in Gregg instead of setting directly
        //Really need to update the logic better so that its not dependant on the room, but positioning
        if (other.tag == "Killer")
        {
            Gregg killer = other.GetComponent<Gregg>();
            //killer.currentRoom = this.transform;
            //killer.adjacentRooms = killer.GetAdjacentRooms(this.transform);
            //killer.nextRoom = killer.SelectNextRoom(killer.adjacentRooms);

            if (meshesEnabled || (litByFlashlight && Player.flashlightOn))
                killer.TurnOnMesh();
            else
                killer.TurnOffMesh();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<Player>().currentRoom != this.transform)
        {
            other.GetComponent<Player>().currentRoom = this.transform;
        }

        if (other.tag == "Killer")
        {
            if (meshesEnabled || (litByFlashlight && Player.flashlightOn))
                other.GetComponent<Gregg>().TurnOnMesh();
            else
                other.GetComponent<Gregg>().TurnOffMesh();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            meshesEnabled = false;
            litByFlashlight = false;

            TurnOffMesh();
        }

        if (other.tag == "Killer")
        {
            if (meshesEnabled || (litByFlashlight && Player.flashlightOn))
                other.GetComponent<Gregg>().TurnOnMesh();
            else
                other.GetComponent<Gregg>().TurnOffMesh();
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

    public void UpdateMeshes()
    {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }
    }
}
