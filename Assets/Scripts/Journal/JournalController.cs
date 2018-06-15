using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {

    public static int journalCount;
    public List<Transform> eventRooms;
    public List<Object> sigils;

    WaypointManager wpm;

    //
    //TO DO: 
    //-Create a list of event rooms in level
    //-Create a list of sigils equal to the number of rooms
    //-Assign a sigil name to each room (and subsiquent doors that the room generated)
    //

    // Use this for initialization
    void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();
        journalCount = 0;
        eventRooms = GetEventRooms(wpm.waypointNodes);
        sigils = SelectRandomSigils(eventRooms);
        AssignSigils(eventRooms, sigils);
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

    List<Transform> GetEventRooms(List<Transform> roomList)
    {
        List<Transform> availableRooms = new List<Transform>();

        foreach (Transform room in roomList)
        {
            WaypointScript node = room.GetComponent<WaypointScript>();
            if (node.type == WaypointScript.Type.eventRoom)
                availableRooms.Add(room);
        }

        return availableRooms;
    }

    List<Object> SelectRandomSigils(List<Transform> roomList)
    {
        Object[] availableSigils = Resources.LoadAll("Sigils/");
        List<Object> sigilsToUse = new List<Object>();

        for (int i = 0; i < roomList.Count; i++)
        {
            int randNum = Random.Range(0, availableSigils.Length);
            if (!sigilsToUse.Contains(availableSigils[randNum]))
                sigilsToUse.Add(availableSigils[randNum]);
        }

        return sigilsToUse;
    }

    void AssignSigils(List<Transform> roomList, List<Object> sigilList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            DoorManager dm = roomList[i].GetComponentInChildren<DoorManager>();
            int randNum = Random.Range(0, sigilList.Count);

            dm.sigilWord = sigilList[randNum].name;
        }
    }
}
