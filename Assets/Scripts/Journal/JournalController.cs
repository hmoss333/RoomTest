using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour {

    public static bool foundAllJournals = false;
    public List<Transform> eventRooms;
    public List<Object> sigils;
    public List<string> foundSigils;

    WaypointManager wpm;

    // Use this for initialization
    void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();
        eventRooms = GetEventRooms(wpm.waypointNodes);
        sigils = SelectRandomSigils(eventRooms);
        AssignDoorSigils(eventRooms, sigils);
        AssignJournalSigils(GetJournals(), sigils);
	}

    public static void IncrementJournal(string sigilName)
    {
        JournalController jc = GameObject.FindObjectOfType<JournalController>();

        foreach (Object s in jc.sigils)
        {
            if (!jc.foundSigils.Contains(sigilName))//s.name))
            {
                jc.foundSigils.Add(sigilName);//s.name);
                Debug.Log(sigilName);//s.name);
                //show sigil image here
                break;
            }
        }

        if (!foundAllJournals && jc.foundSigils.Count == jc.sigils.Count)
            foundAllJournals = true;
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

    List<JournalInteract> GetJournals()
    {
        List<JournalInteract> availableJournals = new List<JournalInteract>();
        JournalInteract[] activeJournals = GameObject.FindObjectsOfType<JournalInteract>();

        for (int i = 0; i < activeJournals.Length; i++)
        {
            availableJournals.Add(activeJournals[i]);
        }

        return availableJournals;
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

    void AssignDoorSigils(List<Transform> roomList, List<Object> sigilList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            DoorManager dm = roomList[i].GetComponentInChildren<DoorManager>();
            int randNum = Random.Range(0, sigilList.Count);

            dm.sigilWord = sigilList[randNum].name;
        }
    }

    void AssignJournalSigils(List<JournalInteract> journalList, List<Object> sigilList)
    {
        //TO DO:
        //for each journal, assign a sigilword that has been assigned to a room
        //if that sigil has already been used, re-roll

        List<JournalInteract> tempJournals = new List<JournalInteract>();
        tempJournals = journalList;
        Debug.Log("Journals: " + tempJournals.Count);
        List<Object> sigils = new List<Object>();
        sigils = sigilList;
        Debug.Log("Sigils: " + sigils.Count);

        foreach (Object sigil in sigils)
        {
            foreach (JournalInteract journal in tempJournals)
            {
                Debug.Log("Sigil: " + sigil.name + ", Journal: " + journal.sigilWord);
                if (journal.sigilWord == sigil.name)
                {
                    tempJournals.Remove(journal);
                    sigils.Remove(sigil);
                    AssignJournalSigils(tempJournals, sigils);
                    break;
                }
                else
                {
                    journal.sigilWord = sigil.name;
                    break;
                }
            }
        }
    }
}
