using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : InteractParent {

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacted with " + gameObject.name + ".");
    }
}
