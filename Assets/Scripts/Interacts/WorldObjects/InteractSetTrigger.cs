using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSetTrigger : InteractParent {

    public State stateToSet;
    RoomManager room;

    public override void Interact()
    {
        room = GetComponentInParent<RoomManager>();
        SetTrigger(stateToSet);
        base.Interact();
    }

    public void SetTrigger(State newState)
    {
        if (state != State.Destroyed)
        {
            if (state == State.Off)
            {
                state = newState;
                text = "Turned on the " + gameObject.name + ". Wonder what this will do.";
                room.hasActiveInteracts = true;
            }
            else
            {
                state = State.Off;
                text = "Turned off the" + gameObject.name + ". Probably safer that way.";
                room.hasActiveInteracts = false; //this is only for testing, it needs to be a check in the RoomManager class
            }
        }
        else
        {
            text = "Something smashed the" + gameObject.name + ". Theres no way I can use this now.";
        }
    }
}
