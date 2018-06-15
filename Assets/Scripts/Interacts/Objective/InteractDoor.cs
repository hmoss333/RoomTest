using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : InteractParent {

    public DoorManager doorManager;
    public string sigilWord;
    //public bool unlocked = false;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("TO DO: start sigil minigame here");
        Debug.Log("Sigil: " + sigilWord);
        doorManager.roomLocked = false;
        doorManager.DestroyDoors(doorManager.connectedDoors);
    }
}
