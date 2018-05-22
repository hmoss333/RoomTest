using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSetTrigger : InteractParent {

    public State stateToSet;

    public float checkRadius;


    public override void Interact()
    {
        base.Interact();
        //DisplayText();

        SetTrigger(stateToSet);
    }

    public void SetTrigger(State newState)
    {
        if (state == State.Off)
        {
            state = newState;
            Debug.Log("Turned on the " + gameObject.name + ". Wonder what this will do.");
        }
        else
        {
            state = State.Off;
            Debug.Log("Turned off the" + gameObject.name + ". Probably safer that way.");
        }
    }
}
