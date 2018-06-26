using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int step = 0;
    public static int objectiveCount = 0;
    public static int weaponCount = 0;
    public static float objectiveScale = 1;
    public static bool foundKey = false;

    public enum GameState { Playing, Paused, Interacting, Win, Lose }
    public static GameState gameState;

    [Header("Actors")]
    public GameObject[] players;
    public GameObject killer;
    public float timeToWaitForKiller;

    [Header("Objective Item Settings")]
    public GameObject[] objectiveItems;
    public int objectiveItemCount;
    public GameObject[] weaponItems;
    public int weaponItemCount;
    public GameObject[] journalItems;
    public float objectiveItemScale;
    public GameObject basementDoor;

    [Header("Game Messages")]
    public string startGameMessage;
    public string spawnKillerMessage;
    public string foundAllObjectiveItemsMessage;
    public string escapeMessage;
    public string somethingChangedMessage;
    public string hiddenEndingMessage;

    private void Start()
    {
        gameState = GameState.Playing;
    }

    // Update is called once per frame
    void Update () {
        if (gameState == GameState.Playing)
        {
            if (step == 1 && Time.timeSinceLevelLoad > timeToWaitForKiller)
            {
                Debug.Log("Took too long");
                UpdateStep();
            }

            if (step == 3 && foundKey && JournalController.foundAllJournals)
            {
                Debug.Log("Found all the secrets");
                StartCoroutine(SomethingChanged());
            }
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
                GameObject[] objectiveItems = gm.objectiveItems;
                GameObject[] players = gm.players;

                Step1(players, objectiveItems, gm.objectiveItemCount, gm.objectiveItemScale, wpm.waypointNodes);
                SpawnObjectives(gm.weaponItems, wpm.waypointNodes, gm.weaponItemCount, gm.objectiveItemScale); //Just testing for now; we can use this as a way to initialize all objects as needed
                SpawnObjectives(gm.journalItems, wpm.waypointNodes, GetEventRooms(wpm.waypointNodes), gm.objectiveItemScale); //spawning journal objects
                break;
            case 2:
                Debug.Log("Spawn killer");
                //Once the players have found the first objectives, killer will spawn and start wandering the house
                GameObject killer = gm.killer;   
                
                Step2(gm.killer, wpm.waypointNodes);
                break;
            case 3:
                Debug.Log("Get to the exit");
                //Step3(gm.killer, wpm.waypointNodes);
                //Player can now leave the house/fix the Van in order to escape
                //Key is now optional; add an outdoors area where the player has to "fix" the car (minigame)
                break;
            case 4:
                //Player has used the main door to escape
                gameState = GameState.Win;
                break;
            case 5:
                //Player has spawned door to basement somewhere on the first floor
                break;
            case 6:
                //Secret ending; become the killer
                break;
            default:
                Debug.Log("Something broke here");
                break;
        }

        UpdateText();
        Resources.UnloadUnusedAssets();
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
            case 4:
                currentMessage = gm.escapeMessage;
                break;
            case 5:
                //currentMessage = gm.somethingChangedMessage;
                break;
            case 6:
                currentMessage = gm.hiddenEndingMessage;
                break;
            default:
                break;
        }

        tc.DisplayText(currentMessage);
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

    static void SpawnObjectives(GameObject[] objItem, List<Transform> roomList, int count, float scale)
    {
        WaypointManager wpm = GameObject.FindObjectOfType<WaypointManager>();
        List<Transform> tempList = new List<Transform>();
        tempList.AddRange(roomList);

        for (int i = count; i > 0; i--)
        {
            int randNum = Random.Range(0, tempList.Count);
            Transform targetRoom = tempList[randNum];
            RoomManager roomScript = targetRoom.GetComponentInChildren<RoomManager>();
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
                    SpawnObjectives(objItem, tempList, i, scale);
                    break;
                }
                else
                {
                    GameObject objPrefab;
                    try
                    {
                        objPrefab = Instantiate(objItem[i], randPos, Quaternion.identity);
                    }
                    catch
                    {
                        objPrefab = Instantiate(objItem[0], randPos, Quaternion.identity);
                        //Debug.Log("Item list out of range");
                    }
                    objPrefab.transform.localScale = Vector3.one * scale;
                    objPrefab.transform.parent = roomScript.gameObject.transform;
                    roomScript.UpdateMeshes();

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

        foreach (Transform node in tempList)
        {
            RoomManager room = node.GetComponentInChildren<RoomManager>();
            if (room.meshesEnabled)
            {
                WaypointScript waypoint = node.GetComponent<WaypointScript>();
                int randNum = Random.Range(0, waypoint.adjactentNodes.Count);
                Vector3 adjacentRoom = waypoint.adjactentNodes[randNum].position;
                killer = Instantiate(killer, new Vector3(adjacentRoom.x, adjacentRoom.y + 1 - (WaypointManager.scale / 4), adjacentRoom.z), Quaternion.identity) as GameObject;
                break;
            }
        }

        //int randNum = Random.Range(0, tempList.Count);
        //Transform targetRoom = tempList[randNum];
        //RoomManager roomScript = targetRoom.GetComponentInChildren<RoomManager>();

        //if (roomScript.meshesEnabled || (roomScript.litByFlashlight && Player.flashlightOn))
        //{
        //    SpawnKiller(killer, tempList);
        //}
        //else
        //{
        //    killer = Instantiate(killer, new Vector3(targetRoom.position.x, targetRoom.position.y + 1 - (WaypointManager.scale / 4), targetRoom.position.z), Quaternion.identity) as GameObject;
        //}
    }

    static void MoveKillerToStart(GameObject killer, List<Transform> roomList)
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

        killer.transform.position = new Vector3(startPos.position.x, 1 - (WaypointManager.scale / 4), startPos.position.z);
    }

    static void SpawnBasementDoor(GameObject basementDoor, List<Transform> roomList)
    {
        List<Transform> tempRooms = new List<Transform>();
        tempRooms = roomList;

        int randNum = Random.Range(0, roomList.Count);
        RoomManager rm = roomList[randNum].GetComponentInChildren<RoomManager>();
        WaypointScript ws = roomList[randNum].GetComponent<WaypointScript>();

        if (ws.zPos == 0)
        {
            if (ws.type != WaypointScript.Type.start && ws.type != WaypointScript.Type.stairs)
            {
                basementDoor = Instantiate(basementDoor, new Vector3(roomList[randNum].position.x, roomList[randNum].position.y + 1 - (WaypointManager.scale / 4), roomList[randNum].position.z), Quaternion.identity) as GameObject;
                basementDoor.transform.localScale = Vector3.one * WaypointManager.scale / 3;
                basementDoor.transform.parent = rm.gameObject.transform;
                rm.UpdateMeshes();
            }
            else
            {
                tempRooms.Remove(roomList[randNum]);
                SpawnBasementDoor(basementDoor, tempRooms);
            }
        }
        else
        {
            tempRooms.Remove(roomList[randNum]);
            SpawnBasementDoor(basementDoor, tempRooms);
        }
    }

    static void Step1(GameObject[] players, GameObject[] objItems, int objCount, float objScale, List<Transform> roomList)
    {
        SpawnPlayers(players, roomList);
        SpawnObjectives(objItems, roomList, objCount, objScale);
    }

    static void Step2(GameObject killer, List<Transform> roomList)
    {
        SpawnKiller(killer, roomList);
    }

    static void Step3(GameObject killer, List<Transform> roomList)
    {
        MoveKillerToStart(killer, roomList);
    }

    static void Step4(GameObject basementDoor, List<Transform> roomList)
    {
        SpawnBasementDoor(basementDoor, roomList);
    }

    IEnumerator SomethingChanged()
    {
        WaypointManager wpm = FindObjectOfType<WaypointManager>();
        TextController tc = GameObject.FindObjectOfType<TextController>();

        step = 5;
        yield return new WaitForSeconds(10);
        Debug.Log("Unlock cool stuff here");
        Step4(basementDoor, wpm.waypointNodes);
        tc.DisplayText(somethingChangedMessage);
    }
}
