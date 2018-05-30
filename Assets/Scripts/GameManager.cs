using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static int step;

    public GameObject[] players;

    //GameManager gm;
    WaypointManager wpm;
    
    // Use this for initialization
	void Start () {
        //gm = GetComponent<>
        wpm = FindObjectOfType<WaypointManager>();
        step = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateStep()
    {
        step++;

        switch (step)
        {
            case 1:
                //Players spawn at start node
                //generate objective item
                SpawnPlayers(players, wpm.waypointNodes);
                break;
            case 2:
                //Players find objective item
                //Spawn Killer
                //Set door to locked
                //Spawn key item (may require players interacting with different objects to find the key
                break;
            case 3:
                //Collect key
                //Players can use door to leave
                //If killer is not incapacitated, players will not be able to leave the house
                //Goal of this section is to hurt killer enough that he enters "stunned" mode so players can use exit (can also possibly trap killer in room; killer will not unspawn during this phase)
                break;
            case 4:
                //If players have followed clues, start triggers for secret boss fight
                break;
            case 5:
                //Next trigger for secret boss fight
                break;
            case 6:
                //Spawn entrance to secret boss fight area
                break;
            default:
                Debug.Log("Something broke here");
                break;
        }
    }

    void SpawnPlayers(GameObject[] players, List<Transform> roomList)
    {
        GameObject[] tempPlayers = null;
        int randNum = Random.Range(0, roomList.Count);

        for (int i = 0; i < players.Length; i++)
        {
            tempPlayers[i] = Instantiate(players[i], new Vector3(roomList[randNum].position.x, 1 - (WaypointManager.scale / 4), roomList[randNum].position.z), Quaternion.identity);
        }
    }

    void SpawnObjectives(GameObject[] objectiveItems, List<Transform> roomList)
    {

    }
}
