using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int step = 0;

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
        //step = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void UpdateStep()
    {
        WaypointManager wpm = FindObjectOfType<WaypointManager>();
        GameManager gm = FindObjectOfType<GameManager>();

        switch (step)
        {
            case 0:
                GameObject objectiveItem = gm.objectiveItem;
                GameObject[] players = gm.players;

                Step1(players, objectiveItem, wpm.waypointNodes);
                break;
            case 1:
                objectiveItem = gm.crowbar;
                GameObject killer = gm.killer;
                
                Step2(killer, objectiveItem, wpm.waypointNodes);
                break;
            case 2:
                //Players now needs key to unlock the door
                //Spawn key item (may require players interacting with different objects to collect)
                //Create ladder object to access second floor
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

        step++;
    }

    static void SpawnObjectives(GameObject objItem, List<Transform> roomList)
    {
        int randNum = Random.Range(0, roomList.Count);
        Transform targetRoom = roomList[randNum];
        HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();

        if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn))
        {
            SpawnObjectives(objItem, roomList);
        }
        else
        {
            objItem = Instantiate(objItem, new Vector3(targetRoom.position.x, 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject;
            objItem.transform.parent = roomScript.gameObject.transform;
            roomScript.UpdateMeshes();
            Debug.Log("Objective in Room: " + targetRoom.GetComponent<WaypointScript>().xPos + ", " + targetRoom.GetComponent<WaypointScript>().yPos);
        }
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
        Transform targetRoom = roomList[randNum];
        HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();

        if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn))
        {
            SpawnKiller(killer, roomList);
        }
        else
        {
            killer = Instantiate(killer, new Vector3(targetRoom.position.x, 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject; //testing for now; need to move to GameManager
            Debug.Log("Killer in Room: " + targetRoom.GetComponent<WaypointScript>().xPos + ", " + targetRoom.GetComponent<WaypointScript>().yPos);
        }
    }

    static void Step1(GameObject[] players, GameObject objItem, List<Transform> roomList)
    {
        SpawnPlayers(players, roomList);
        SpawnObjectives(objItem, roomList);
    }

    static void Step2(GameObject killer, GameObject objItem, List<Transform> roomList)
    {
        SpawnKiller(killer, roomList);
        SpawnObjectives(objItem, roomList);
    }
}
