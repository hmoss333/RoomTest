using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    public bool litByFlashlight;
    Color col;

    public InteractSetTrigger[] interacts;
    public bool hasActiveInteracts;

    // Use this for initialization
    void Start()
    {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();
        litByFlashlight = false;
        hasActiveInteracts = false;

        foreach (MeshRenderer mesh in roomMeshes)
            mesh.enabled = false;

        meshesEnabled = false;

        interacts = gameObject.GetComponentsInChildren<InteractSetTrigger>();
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

        if (hasActiveInteracts)
            hasActiveInteracts = GetDestroyedInteracts(interacts);
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
            killer.currentRoom = this;

            if (meshesEnabled || (litByFlashlight && Player.flashlightOn))
                killer.TurnOnMesh();
            else
                killer.TurnOffMesh();

            //if (hasActiveInteracts)
            //{
            //    foreach (InteractSetTrigger trigger in interacts)
            //    {
            //        trigger.state = InteractParent.State.Disabled;
            //        hasActiveInteracts = false;
            //    }
            //}
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

    bool GetDestroyedInteracts (InteractSetTrigger[] interactList)
    {
        for (int i = 0; i < interactList.Length; i++)
        {
            if (interactList[i].state == InteractParent.State.Destroyed)
                return false;
        }

        return true;
    }
}
