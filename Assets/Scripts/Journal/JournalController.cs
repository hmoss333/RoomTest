using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalController : MonoBehaviour {

    public Image background;

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

        if (!jc.foundSigils.Contains(sigilName))
            jc.foundSigils.Add(sigilName);

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
            dm.sigilImage = Resources.Load<Sprite>("SigilImages/" + sigilList[randNum].name);
        }
    }

    void AssignJournalSigils(List<JournalInteract> journalList, List<Object> sigilList)
    {
        List<JournalInteract> tempJournals = new List<JournalInteract>();
        tempJournals = journalList;
        List<Object> tempSigils = new List<Object>();
        tempSigils = sigilList;

        for (int i = 0; i < tempSigils.Count; i++)
        {
            for (int j = 0; j < tempJournals.Count; j++)
            {
                if (tempJournals[j].sigilWord != tempSigils[i].name) //if the current journal does not have the current sigil
                {
                    tempJournals[j].sigilWord = tempSigils[i].name;
                    tempJournals[j].sigilImage = Resources.Load<Sprite>("SigilImages/" + tempSigils[i].name) as Sprite;
                    tempJournals.Remove(tempJournals[j]);
                    break;
                }
            }
        }

        foreach (JournalInteract journal in tempJournals)
        {
            if (journal.sigilWord == "") //if any sigils are left unnassigned after first-pass, run again
                AssignJournalSigils(tempJournals, tempSigils);
        }
    }

    void AssignSigilImages(List<Transform> objectList, List<Object> sigilList)
    {

    }
}
