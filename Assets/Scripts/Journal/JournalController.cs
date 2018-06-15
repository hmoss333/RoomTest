using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {

    public List<Transform> eventRooms;
    public List<Object> sigils;
    public List<string> foundSigils;

    WaypointManager wpm;

    // Use this for initialization
    void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();
        eventRooms = GetEventRooms(wpm.waypointNodes);
        sigils = SelectRandomSigils(eventRooms);
        AssignSigils(eventRooms, sigils);
	}

    public static void IncrementJournal()
    {
        JournalController jc = GameObject.FindObjectOfType<JournalController>();

        foreach (Object s in jc.sigils)
        {
            if (!jc.foundSigils.Contains(s.name))
            {
                jc.foundSigils.Add(s.name);
                Debug.Log(s.name);
                //show sigil image here
                break;
            }
        }
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
