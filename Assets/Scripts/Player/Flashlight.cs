using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    public float rotationSpeed = 1;
    public float viewDistance = 10f;
    RaycastHit foundHit;

    Player player;
    WaypointManager wpm;


    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<Player>();
        wpm = GameObject.FindObjectOfType<WaypointManager>();
	}
	
	// Update is called once per frame
	void Update () {
        try
        {
            StopLookAtRoom();

            //if (Player.flashlightOn)
            //{
            Quaternion rot = Quaternion.LookRotation(player.lastDir, player.transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            LookAtRoom();
            //}
        }
        catch { }
    }

    void LookAtRoom()
    {
        foundHit = new RaycastHit();
        bool test = Physics.Raycast(transform.position, player.lastDir, out foundHit, viewDistance, 1 << LayerMask.NameToLayer("Room"));
        Debug.DrawRay(transform.position, player.lastDir, Color.green);

        if (test)
        {
            foundHit.transform.GetComponent<RoomManager>().litByFlashlight = true;
        }
    }

    public void StopLookAtRoom()
    {
        foreach (Transform node in wpm.waypointNodes)
        {
            RoomManager room = node.GetComponentInChildren<RoomManager>();

            if (room.transform != player.currentRoom)
            {
                room.litByFlashlight = false;
            }
        }
    }
}
