using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : InteractParent {

    public DoorManager doorManager;
    //public bool unlocked = false;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        base.Interact();
        Debug.Log("TO DO: start sigil minigame here");
        doorManager.roomLocked = false;
        doorManager.DestroyDoors(doorManager.connectedDoors);
    }
}
