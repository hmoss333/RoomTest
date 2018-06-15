using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjective : InteractParent {

    GameManager gm;

    public override void Start()
    {
        base.Start();
        gm = GameObject.FindObjectOfType<GameManager>();
    }

    public override void Interact()
    {
        base.Interact();
        if (GameManager.step == 1)
            GameManager.UpdateStep();

        GameManager.objectiveCount++;
        Debug.Log("Objective Items: " + GameManager.objectiveCount);

        if (GameManager.objectiveCount == gm.objectiveItemCount && GameManager.step == 2)
        {
            GameManager.UpdateStep();
        }

        //Disabling object for now, but probably want to add this to a list or something
        //Destroy(this.gameObject);
        //GetComponentInParent<HideRoom>().UpdateMeshes();
        this.gameObject.SetActive(false);
    }
}
