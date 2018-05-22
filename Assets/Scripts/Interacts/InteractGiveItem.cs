using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGiveItem : InteractParent {

    public GameObject itemToGive;

    public override void Interact()
    {
        base.Interact();
        //DisplayText();

        GiveItem(itemToGive);
    }

    public void GiveItem(GameObject item)
    {
        Debug.Log("Found " + item.name + ". Maybe this will come in handy.");
    }
}
