﻿using System.Collections;
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
        if (objectiveCount == objectiveItemCount && step == 2)
        {
            UpdateStep();
        }
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

                Step1(players, objectiveItem, gm.objectiveItemCount, wpm.waypointNodes);
                SpawnObjectives(gm.crowbar, wpm.waypointNodes, 2); //Just testing for now; we can use this as a way to initialize all objects as needed
                break;
            case 1:
                GameObject killer = gm.killer;   
                
                Step2(gm.killer, wpm.waypointNodes);
                break;
            case 2:
                //door unlocks
                //Teleport killer to start room
                killer = gm.killer;

                MoveKiller(gm.killer, wpm.waypointNodes);
                break;
            case 3:
                //If killer is not incapacitated, players will not be able to leave the house
                //Goal of this section is to hurt killer enough that he enters "stunned" mode so players can use exit (can also possibly trap killer in room; killer will not unspawn during this phase)
                killer = gm.killer;

                MoveKiller(gm.killer, wpm.waypointNodes);
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
        //Debug.Log("Current Step: " + step);
    }

    static void SpawnObjectives(GameObject objItem, List<Transform> roomList, int count)
    {
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        for (int i = count; i > 0; i--)
        {
            int randNum = Random.Range(0, tempList.Count);
            Transform targetRoom = tempList[randNum];
            HideRoom roomScript = targetRoom.GetComponentInChildren<HideRoom>();

            if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn))
            {
                SpawnObjectives(objItem, tempList, i);
            }
            else
            {
                Vector3 randPos = new Vector3(
                    targetRoom.position.x + Random.Range(-WaypointManager.scale / 2, WaypointManager.scale / 2), 
                    1 - (WaypointManager.scale / 4), 
                    targetRoom.position.z + Random.Range(-WaypointManager.scale / 2, WaypointManager.scale / 2)
                    );

                objItem = Instantiate(objItem, randPos, Quaternion.identity);
                objItem.transform.localScale = new Vector3(1, 1, 1);
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
            killer = Instantiate(killer, new Vector3(targetRoom.position.x, 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject; //testing for now; need to move to GameManager
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
                Debug.Log(nodeData.name);
            }
        }

        Destroy(GameObject.Find(killer.name + "(Clone)"));
        killer = Instantiate(killer, new Vector3(startPos.position.x, 1 - (WaypointManager.scale / 4), startPos.position.z), Quaternion.identity) as GameObject;
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
