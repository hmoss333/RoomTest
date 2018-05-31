using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int step;

    public GameObject[] players;
    public GameObject killer;
    public GameObject objectiveItem;
    public GameObject crowbar;

    //GameManager gm;
    //WaypointManager wpm;
    
    // Use this for initialization
	void Start () {
        //gm = GetComponent<GameManager>();
        //wpm = FindObjectOfType<WaypointManager>();
        step = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void UpdateStep()
    {
        //step++;
        Debug.Log(step);

        WaypointManager wpm = FindObjectOfType<WaypointManager>();
        GameManager gm = FindObjectOfType<GameManager>();

        switch (step)
        {
            case 0:
                GameObject objectiveItem = gm.objectiveItem;
                GameObject[] players = gm.players;

                Step1(objectiveItem, players, wpm.waypointNodes);
                break;
            case 1:
                objectiveItem = gm.crowbar;
                GameObject killer = gm.killer;
                
                Step2(objectiveItem, killer, wpm.waypointNodes);
                break;
            case 2:
                //Players now needs key to unlock the door
                //Spawn key item (may require players interacting with different objects to collect)
                break;
            case 3:
                //If killer is not incapacitated, players will not be able to leave the house
                //Goal of this section is to hurt killer enough that he enters "stunned" mode so players can use exit (can also possibly trap killer in room; killer will not unspawn during this phase)
                break;
            case 4:
                //If players have followed clues, start triggers for secret boss fight
                //Next trigger for secret boss fight
                break;
            case 5:
                //Spawn entrance to secret boss fight area
                break;
            default:
                Debug.Log("Something broke here");
                break;
        }
    }

    static void SpawnObjectives(GameObject objectiveItem, List<Transform> roomList)
    {
        int randNum = Random.Range(0, roomList.Count);
        Transform targetRoom = roomList[randNum];
        HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();

        objectiveItem = Instantiate(objectiveItem, new Vector3(targetRoom.position.x, 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject;
        objectiveItem.transform.parent = roomScript.gameObject.transform;
        roomScript.UpdateMeshes();
    }

    static void SpawnPlayers(GameObject[] players, List<Transform> roomList)
    {
        int randNum = Random.Range(0, roomList.Count);

        Transform startPos = null;
        foreach (Transform currentNode in roomList)
        {
            WaypointScript nodeData = currentNode.GetComponentInChildren<WaypointScript>();

            if (nodeData.type == WaypointScript.Type.start)
            {
                startPos = nodeData.transform;
                Debug.Log(nodeData.name);
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = Instantiate(players[i], new Vector3(startPos.position.x, 1 - (WaypointManager.scale / 4), startPos.position.z), Quaternion.identity);
        }
    }

    static void SpawnKiller(GameObject killer, List<Transform> roomList)
    {
        int randNum = Random.Range(0, roomList.Count);
        killer = Instantiate(killer, new Vector3(roomList[randNum].position.x, 1 - (WaypointManager.scale / 4), roomList[randNum].position.z), Quaternion.identity); //testing for now; need to move to GameManager
    }

    static void Step1(GameObject objectiveItem, GameObject[] players, List<Transform> roomList)
    {
        SpawnObjectives(objectiveItem, roomList);
        SpawnPlayers(players, roomList);
    }

    static void Step2(GameObject objectiveItem, GameObject killer, List<Transform> roomList)
    {
        SpawnObjectives(objectiveItem, roomList);
        SpawnKiller(killer, roomList);
    }
}
