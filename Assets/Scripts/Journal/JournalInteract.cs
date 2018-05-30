using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalInteract : InteractParent {

    public bool collected;
    
    // Use this for initialization
	void Start () {
        collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        base.Interact();
        if (!collected)
        {
            JournalController.IncrementJournal();
            collected = true;
        }
    }
}
