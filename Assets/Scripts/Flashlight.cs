﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    Player player;
    public float rotationSpeed = 1; //
    Vector3 relativePos;

    WaypointManager wpm;
    
    // Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
        wpm = GameObject.FindObjectOfType<WaypointManager>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (player.direction)
        {
            case Player.Direction.Up:
                relativePos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1);
                break;
            case Player.Direction.Down:
                relativePos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1);
                break;
            case Player.Direction.Left:
                relativePos = new Vector3(player.transform.position.x - 1, player.transform.position.y, player.transform.position.z);
                break;
            case Player.Direction.Right:
                relativePos = new Vector3(player.transform.position.x + 1, player.transform.position.y, player.transform.position.z);
                break;
            default:
                break;
        }

        Vector3 dir = relativePos - transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);


        foreach (Transform node in wpm.waypointNodes)
        {
            HideRoom currentNode = node.GetComponentInChildren<HideRoom>();

            if (currentNode.meshesEnabled)
            {
                WaypointScript currentRoom = currentNode.GetComponentInParent<WaypointScript>();

                foreach (WaypointScript room in currentNode.adjactentRooms)
                {
                    if (room.yPos == currentRoom.yPos + 1 && player.direction == Player.Direction.Up)
                    {
                        room.GetComponentInChildren<HideRoom>().TurnOnMesh();
                    }
                    else if (room.yPos == currentRoom.yPos - 1 && player.direction == Player.Direction.Down)
                    {
                        room.GetComponentInChildren<HideRoom>().TurnOnMesh();
                    }
                    else if (room.xPos == currentRoom.xPos + 1 && player.direction == Player.Direction.Right)
                    {
                        room.GetComponentInChildren<HideRoom>().TurnOnMesh();
                    }
                    else if (room.xPos == currentRoom.xPos - 1 && player.direction == Player.Direction.Left)
                    {
                        room.GetComponentInChildren<HideRoom>().TurnOnMesh();
                    }
                    else
                    {
                        room.GetComponentInChildren<HideRoom>().TurnOffMesh();
                    }
                }
            }
        }
    }
}
