using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInteract : InteractParent {

    public bool collected;
    
    // Use this for initialization
	public override void Start () {
        base.Start();
        collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        //base.Interact();
        if (!collected)
        {
            base.Interact();
            JournalController.IncrementJournal();
            collected = true;
        }

        //Display journal UI popup
        //Pause gameplay until user presses a button
        //Should re-display journal UI every time they interact with this item
    }
}
