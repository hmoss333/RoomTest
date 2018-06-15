using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : InteractParent {

    public DoorManager doorManager;
    public string sigilWord;
    //public bool unlocked = false;
    JournalController jc;

    public override void Start()
    {
        base.Start();
        jc = GameObject.FindObjectOfType<JournalController>();
    }

    public override void Interact()
    {
        Debug.Log("TO DO: start sigil minigame here");
        if (jc.foundSigils.Contains(sigilWord))
        {
            doorManager.roomLocked = false;
            doorManager.DestroyDoors(doorManager.connectedDoors);
            text = "The symbol from the journal unlocked the door";
        }
        else
        {
            text = "I don't see anywhere for a key, just a weird symbol...";
        }

        base.Interact();
    }
}
