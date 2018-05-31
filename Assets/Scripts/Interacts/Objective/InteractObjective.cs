using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjective : InteractParent {

    public override void Interact()
    {
        base.Interact();
        GameManager.step++;
        GameManager.UpdateStep();

        //Disabling object for now, but probably want to add this to a list or something
        //Destroy(this.gameObject);
        //GetComponentInParent<HideRoom>().UpdateMeshes();
        this.gameObject.SetActive(false);
    }
}
