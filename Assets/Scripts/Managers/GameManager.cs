using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int step = 0;
    public static int objectiveCount = 0;

    public GameObject[] players;
    public GameObject killer;
    public GameObject objectiveItem;
    public int objectiveItemCount;
    public GameObject crowbar;
    
    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    public static void UpdateStep()
    {
        WaypointManager wpm = FindObjectOfType<WaypointManager>();
        GameManager gm = FindObjectOfType<GameManager>();

        step++;
        Debug.Log(step);

        switch (step)
        {
            case 1:
                Debug.Log("Spawn Player & Objectives");
                GameObject objectiveItem = gm.objectiveItem;
                GameObject[] players = gm.players;

                Step1(players, objectiveItem, gm.objectiveItemCount, wpm.waypointNodes);
                SpawnObjectives(gm.crowbar, wpm.waypointNodes, 2); //Just testing for now; we can use this as a way to initialize all objects as needed
                break;
            case 2:
                Debug.Log("Spawn killer");
                //Once the players have found the first objectives, killer will spawn and start wandering the house
                GameObject killer = gm.killer;   
                
                Step2(gm.killer, wpm.waypointNodes);
                break;
            case 3:
                Debug.Log("Spawn Key");
                //Players need to find the key in order to get out
                //This case can actually just be removed/replaced as its only a placeholder for now
                break;
            case 4:
                //Once the key is found, if killer is not incapacitated, players will not be able to leave the house
                //Goal of this section is to hurt killer enough that he enters "stunned" mode so players can use exit (can also possibly trap killer in room; killer will not unspawn during this phase)
                killer = gm.killer;

                MoveKiller(gm.killer, wpm.waypointNodes);
                killer.GetComponent<Gregg>().moveToNextRoom = false;
                break;
            case 5:
                //If players have followed clues, start triggers for secret boss fight
                //Next trigger for secret boss fight
                break;
            case 6:
                //Spawn entrance to secret boss fight area
                break;
            default:
                Debug.Log("Something broke here");
                break;
        }

        //step++;
        //Debug.Log("Current Step: " + step);
    }

    static void SpawnObjectives(GameObject objItem, List<Transform> roomList, int count)
    {
        WaypointManager wpm = GameObject.FindObjectOfType<WaypointManager>();
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        for (int i = count; i > 0; i--)
        {
            int randNum = Random.Range(0, tempList.Count);
            Transform targetRoom = tempList[randNum];
            HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();
            WaypointScript room = targetRoom.GetComponent<WaypointScript>();

            if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn) || room.type == WaypointScript.Type.stairs)
            {
                SpawnObjectives(objItem, tempList, i);
            }
            else
            {
                //Removing the randomized positions for the time being so that I can determine a better way to populate each room
                Vector3 randPos = new Vector3(
                    targetRoom.position.x /*+ Random.Range(-WaypointManager.scale / 3, WaypointManager.scale / 3)*/, 
                    targetRoom.position.y + 1 - (WaypointManager.scale / 4), 
                    targetRoom.position.z /*+ Random.Range(-WaypointManager.scale / 3, WaypointManager.scale / 3)*/
                    );

                objItem = Instantiate(objItem, randPos, Quaternion.identity);
                objItem.transform.localScale = new Vector3(2, 2, 2);
                objItem.transform.parent = roomScript.gameObject.transform;
                roomScript.UpdateMeshes();
                //Debug.Log("Objective in Room: " + targetRoom.GetComponent<WaypointScript>().xPos + ", " + targetRoom.GetComponent<WaypointScript>().yPos);
            }

            tempList.Remove(targetRoom);
        }
    }

    static void SpawnPlayers(GameObject[] players, List<Transform> roomList)
    {
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        Transform startPos = null;
        foreach (Transform currentNode in tempList)
        {
            WaypointScript nodeData = currentNode.GetComponentInChildren<WaypointScript>();

            if (nodeData.type == WaypointScript.Type.start)
            {
                startPos = nodeData.transform;
                //Debug.Log(nodeData.name);
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = Instantiate(players[i], new Vector3(startPos.position.x, 1 - (WaypointManager.scale / 4), startPos.position.z), Quaternion.identity);
        }
    }

    static void SpawnKiller(GameObject killer, List<Transform> roomList)
    {
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        int randNum = Random.Range(0, tempList.Count);
        Transform targetRoom = tempList[randNum];
        HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();

        if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn))
        {
            SpawnKiller(killer, tempList);
        }
        else
        {
            killer = Instantiate(killer, new Vector3(targetRoom.position.x, targetRoom.position.y + 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject; //testing for now; need to move to GameManager
            //Debug.Log("Killer in Room: " + targetRoom.GetComponent<WaypointScript>().xPos + ", " + targetRoom.GetComponent<WaypointScript>().yPos);
        }
    }

    //Debating using this at the moment; will leave for now, but probably can design a gameplay reason to be unable to leave without moving the killer to the ext
    static void MoveKiller(GameObject killer, List<Transform> roomList)
    {
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        Transform startPos = null;
        foreach (Transform currentNode in tempList)
        {
            WaypointScript nodeData = currentNode.GetComponentInChildren<WaypointScript>();

            if (nodeData.type == WaypointScript.Type.start)
            {
                startPos = nodeData.transform;
                //Debug.Log(nodeData.name);
            }
        }

        Destroy(GameObject.Find(killer.name + "(Clone)"));
        killer = Instantiate(killer, new Vector3(startPos.position.x, 1 - (WaypointManager.scale / 4), startPos.position.z), Quaternion.identity) as GameObject;
        killer.GetComponent<Gregg>().moveToNextRoom = false; //Leaving for now, we'll come back to all of the killer logic once we get pathfinding installed
        Debug.Log("Killer in Start Room: " + startPos.GetComponent<WaypointScript>().xPos + ", " + startPos.GetComponent<WaypointScript>().yPos);
    }

    static void Step1(GameObject[] players, GameObject objItem, int objCount, List<Transform> roomList)
    {
        SpawnPlayers(players, roomList);
        SpawnObjectives(objItem, roomList, objCount);
    }

    static void Step2(GameObject killer, List<Transform> roomList)
    {
        SpawnKiller(killer, roomList);
    }
}
