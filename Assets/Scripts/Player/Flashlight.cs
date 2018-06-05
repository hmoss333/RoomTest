using System.Collections;
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
        StopLookAtRoom();

        if (Player.flashlightOn)
        {
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

            LookAtRoom(player.currentRoom);
        }
    }

    public void LookAtRoom(Transform roomToLookAt)
    {
        Player playerClass = GameObject.FindObjectOfType<Player>();
        WaypointScript currentRoom = roomToLookAt.GetComponentInParent<WaypointScript>();

        foreach (Transform room in currentRoom.adjactentNodes)
        {
            room.GetComponentInChildren<HideRoom>().litByFlashlight = false;

            WaypointScript roomPos = room.GetComponent<WaypointScript>();
            if (roomPos.zPos == currentRoom.zPos)
            {
                switch (playerClass.direction)
                {
                    case Player.Direction.Up:
                        if (roomPos.yPos == currentRoom.yPos + 1 && roomPos.xPos == currentRoom.xPos)
                            room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        break;
                    case Player.Direction.Left:
                        if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos - 1)
                            room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        break;
                    case Player.Direction.Right:
                        if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                            room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        break;
                    case Player.Direction.Down:
                        if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                            room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        break;
                    default:
                        room.GetComponentInChildren<HideRoom>().litByFlashlight = false;
                        break;
                }

            }
        }
    }

    public void StopLookAtRoom()
    {
        Player playerClass = GameObject.FindObjectOfType<Player>();
        //WaypointScript currentRoom = roomToLookAt.GetComponentInParent<WaypointScript>();

        foreach (Transform node in wpm.waypointNodes)
        {
            HideRoom room = node.GetComponentInChildren<HideRoom>();

            if (room.transform != playerClass.currentRoom)
            {
                room.litByFlashlight = false;
            }
        }
    }
}
