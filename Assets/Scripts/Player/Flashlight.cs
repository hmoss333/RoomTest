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
            Quaternion rot = Quaternion.LookRotation(player.lastDir, player.transform.up);
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
                        if (player.transform.rotation.eulerAngles.y == 0)
                        {
                            if (roomPos.yPos == currentRoom.yPos + 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 270)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos - 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 90)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 180)
                        {
                            if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        break;
                    case Player.Direction.Left:
                        if (player.transform.rotation.eulerAngles.y == 0)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos - 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 270)
                        {
                            //if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                            if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 90)
                        {
                            //if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                            if (roomPos.yPos == currentRoom.yPos + 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 180)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        break;
                    case Player.Direction.Right:
                        if (player.transform.rotation.eulerAngles.y == 0)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 270)
                        {
                            if (roomPos.yPos == currentRoom.yPos + 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 90)
                        {
                            if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 180)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos - 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        break;
                    case Player.Direction.Down:
                        if (player.transform.rotation.eulerAngles.y == 0)
                        {
                            if (roomPos.yPos == currentRoom.yPos - 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 270)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos + 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 90)
                        {
                            if (roomPos.yPos == currentRoom.yPos && roomPos.xPos == currentRoom.xPos - 1)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
                        else if (player.transform.rotation.eulerAngles.y == 180)
                        {
                            if (roomPos.yPos == currentRoom.yPos + 1 && roomPos.xPos == currentRoom.xPos)
                                room.GetComponentInChildren<HideRoom>().litByFlashlight = true;
                        }
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
