using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {

    public static int journalCount;
    
    // Use this for initialization
	void Start () {
        journalCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void IncrementJournal()
    {
        journalCount++;

        //
        //TO DO: Add journal logic here
        //
    }
}
