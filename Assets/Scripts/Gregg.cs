using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gregg : MonoBehaviour {

    [Header("Movement Settings")]
    public float walkSpeed;
    public float hurtSpeed;
    public float attackSpeed;
    float speed;
    public Transform currentRoom;
    public Transform nextRoom;
    public bool moveToNextRoom;
    public List<Transform> adjacentRooms;

    [Header("Interact Settings")]
    public float checkDist;
    public float checkTime;
    RaycastHit foundHit;

    [Header("Controls References")]
    Rigidbody rb;
    Animator animator;

    [Header("Attack Settings")]
    public GameObject weaponPrefab;
    public float attackTime;

    SpriteRenderer[] characterRenderer;

    // Use this for initialization
    void Start () {
        speed = walkSpeed;
        moveToNextRoom = false;

        //nextRoom = SelectNextRoom(GetAdjacentRooms(currentRoom)); //technically works, but just sets the current room, not actual adjactent
        StartCoroutine(MoveTimer(5f));
        //StartCoroutine(DisapearTimer(45f));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (moveToNextRoom && nextRoom != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(nextRoom.position.x, 1 - (WaypointManager.scale / 4), nextRoom.position.z), speed * Time.deltaTime);
        }
	}

    IEnumerator MoveTimer(float waitTime)
    {
        //moveToNextRoom = false;

        //yield return new WaitForSeconds(waitTime);

        moveToNextRoom = true;

        yield return new WaitForSeconds(waitTime);

        moveToNextRoom = false;

        StartCoroutine(MoveTimer(waitTime));
    }

    IEnumerator DisapearTimer(float deathTime)
    {
        yield return new WaitForSeconds(deathTime);
        //TO DO: send message to GameManager to start respawn timer
        Destroy(this.gameObject);
    }

    //public void TurnOnMesh()
    //{
    //    foreach (SpriteRenderer mesh in characterRenderer)
    //        mesh.enabled = true;
    //}

    //public void TurnOffMesh()
    //{
    //    foreach (SpriteRenderer mesh in characterRenderer)
    //        mesh.enabled = false;
    //}

    public List<Transform> GetAdjacentRooms(Transform currentActiveRoom)
    {
        List<Transform> tempRoomList = new List<Transform>();

        WaypointScript thisNode = currentActiveRoom.GetComponentInParent<WaypointScript>();
        WaypointScript[] roomList = GameObject.FindObjectsOfType<WaypointScript>();
        foreach (WaypointScript room in roomList)
        {
            if (room != thisNode)
            {
                //This will only include rooms immediately above/bellow/left/right of current room
                if ((room.xPos == thisNode.xPos + 1 && room.yPos == thisNode.yPos)
                    || (room.xPos == thisNode.xPos - 1 && room.yPos == thisNode.yPos)
                    || (room.yPos == thisNode.yPos + 1 && room.xPos == thisNode.xPos)
                    || (room.yPos == thisNode.yPos - 1 && room.xPos == thisNode.xPos))
                {
                    tempRoomList.Add(room.transform);
                }
            }
        }

        return tempRoomList;
    }

    public Transform SelectNextRoom(List<Transform> roomList)
    {
        Transform targetRoom;
        int randNum = Random.Range(0, roomList.Count);

        targetRoom = roomList[randNum];
        return targetRoom;
    }
}
