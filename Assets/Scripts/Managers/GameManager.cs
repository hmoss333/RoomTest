using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int step = 0;
    public static int objectiveCount = 0;
    public static int weaponCount = 0;
    public static float objectiveScale = 1;

    public enum GameState { Playing, Paused, Interacting, Win, Lose }
    public static GameState gameState;

    [Header("Actors")]
    public GameObject[] players;
    public GameObject killer;
    public float timeToWaitForKiller;

    [Header("Objective Item Settings")]
    public GameObject objectiveItem;
    public int objectiveItemCount;
    public GameObject weaponItem;
    public int weaponItemCount;
    public GameObject journalItem;
    public float objectiveItemScale;

    [Header("Objective Item Settings")]
    public string startGameMessage;
    public string spawnKillerMessage;
    public string foundAllObjectiveItemsMessage;
    public string escapeMessage;
    public string hiddenEndingMessage;

    private void Start()
    {
        gameState = GameState.Playing;
    }

    // Update is called once per frame
    void Update () {
        if (gameState == GameState.Playing && step == 1 && Time.timeSinceLevelLoad > timeToWaitForKiller)
        {
            Debug.Log("Took too long");
            UpdateStep();
        }
    }

    public static void UpdateStep()
    {
        WaypointManager wpm = FindObjectOfType<WaypointManager>();
        GameManager gm = FindObjectOfType<GameManager>();

        step++;
        Debug.Log("Step#: " + step);

        switch (step)
        {
            case 1:
                Debug.Log("Spawn Player & Objectives");
                GameObject objectiveItem = gm.objectiveItem;
                GameObject[] players = gm.players;

                Step1(players, objectiveItem, gm.objectiveItemCount, gm.objectiveItemScale, wpm.waypointNodes);
                SpawnObjectives(gm.weaponItem, wpm.waypointNodes, gm.weaponItemCount, gm.objectiveItemScale); //Just testing for now; we can use this as a way to initialize all objects as needed
                SpawnObjectives(gm.journalItem, wpm.waypointNodes, GetEventRooms(wpm.waypointNodes), gm.objectiveItemScale); //spawning journal objects
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

        UpdateText();
    }

    static void UpdateText()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        TextController tc = GameObject.FindObjectOfType<TextController>();
        string currentMessage = null;

        switch (step)
        {
            case 1:
                currentMessage = gm.startGameMessage;
                break;
            case 2:
                currentMessage = gm.spawnKillerMessage;
                break;
            case 3:
                currentMessage = gm.foundAllObjectiveItemsMessage;
                break;
            //case 4:
            //    currentMessage = TextController.textToDisplay;
            //    break;
            case 5:
                currentMessage = gm.escapeMessage;
                break;
            case 6:
                currentMessage = gm.hiddenEndingMessage;
                break;
            default:
                currentMessage = TextController.textToDisplay;
                break;
        }

        TextController.textToDisplay = currentMessage;
        tc.DisplayText();
    }

    static int GetEventRooms(List<Transform> roomList)
    {
        int lockedRooms = 0;

        foreach (Transform room in roomList)
        {
            if (room.GetComponent<WaypointScript>().type == WaypointScript.Type.eventRoom)
                lockedRooms++;
        }

        return lockedRooms;
    }

    static void SpawnObjectives(GameObject objItem, List<Transform> roomList, int count, float scale)
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
                tempList.Remove(targetRoom);
                SpawnObjectives(objItem, tempList, i, scale);
                break;
            }
            else
            {
                //Removing the randomized positions for the time being so that I can determine a better way to populate each room
                Vector3 randPos = new Vector3(
                    targetRoom.position.x + Random.Range(-WaypointManager.scale / 3, WaypointManager.scale / 3), 
                    targetRoom.position.y + 1 - (WaypointManager.scale / 4), 
                    targetRoom.position.z + Random.Range(-WaypointManager.scale / 3, WaypointManager.scale / 3)
                    );

                Collider[] colliders = Physics.OverlapSphere(randPos, scale/2);
                if (colliders.Length > 1) //Room collider
                {
                    //Debug.Log("Objective spawn point on top of something at: " + randPos + "; Number of objects: " + colliders.Length);
                    SpawnObjectives(objItem, tempList, i, scale);
                    break;
                }
                else
                {
                    objItem = Instantiate(objItem, randPos, Quaternion.identity);
                    objItem.transform.localScale = Vector3.one * scale;
                    objItem.transform.parent = roomScript.gameObject.transform;
                    roomScript.UpdateMeshes();
                    //Debug.Log("Spawned objective: " + objItem.name);

                    tempList.Remove(targetRoom);
                }
            }
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
        Debug.Log("Killer moved to Start Room: " + startPos.GetComponent<WaypointScript>().xPos + ", " + startPos.GetComponent<WaypointScript>().yPos);
    }

    static void Step1(GameObject[] players, GameObject objItem, int objCount, float objScale, List<Transform> roomList)
    {
        SpawnPlayers(players, roomList);
        SpawnObjectives(objItem, roomList, objCount, objScale);
    }

    static void Step2(GameObject killer, List<Transform> roomList)
    {
        SpawnKiller(killer, roomList);
    }
}
