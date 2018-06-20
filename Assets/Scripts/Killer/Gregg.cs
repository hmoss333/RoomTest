using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gregg : MonoBehaviour {

    [Header("Movement Settings")]
    public float speed;
    public float attackSpeed;
    float teleportTime;
    public float footprintSpacing = 2.0f; // distance between each footprint
    Vector3 lastPos;
    public bool stunned = false;
    public float stunTimer;

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

    WaypointManager wpm;
    GameManager gm;
    Player player;
    Footprints footprints;
    public RoomManager currentRoom;

    // Use this for initialization
    void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();
        gm = GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindObjectOfType<Player>();

        lastPos = transform.position;

        if (!footprints)
        {
            footprints = GameObject.FindObjectOfType<Footprints>();
        }

        teleportTime = UpdateTeleportTimer(gm.objectiveItemCount, GameManager.objectiveCount, 3f);
        StartCoroutine(TeleportTimer(teleportTime));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //If the room contains an active interact && the player is not currently in the room/shining a light in the room
        //The killer will then target the nearest interact
        if (!stunned)
        {
            if (currentRoom && currentRoom.hasActiveInteracts && (!currentRoom.meshesEnabled || !currentRoom.litByFlashlight))
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentRoom.interacts[Random.Range(0, currentRoom.interacts.Length)].transform.position.x, transform.position.y, currentRoom.interacts[Random.Range(0, currentRoom.interacts.Length)].transform.position.z), speed * Time.deltaTime);
            else
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speed * Time.deltaTime);
        }

        //Footprint logic
        float distFromLastFootprint = (lastPos - transform.position).sqrMagnitude;
        if (distFromLastFootprint > footprintSpacing * footprintSpacing)
        {
            footprints.AddFootprint(transform.position, transform.forward, transform.right, Random.Range(0, 4));

            lastPos = transform.position;
        }

        if (footprints.transform.position.y != transform.position.y)
            footprints.transform.position = new Vector3(0, transform.position.y - 10, 0);
    }

    IEnumerator TeleportTimer(float time)
    {
        yield return new WaitForSeconds(time);

        MoveToNewRoom(wpm.waypointNodes);
        teleportTime = UpdateTeleportTimer(gm.objectiveItemCount, GameManager.objectiveCount, 3f);
        StartCoroutine(TeleportTimer(teleportTime));
    }

    void MoveToNewRoom(List<Transform> roomList)
    {
        if (!GetComponent<MeshRenderer>().enabled) //If killer is currently not visible
        {
            List<Transform> tempList = new List<Transform>();
            tempList.AddRange(roomList);

            //This code feels like it could be written more cleanly
            foreach (Transform node in tempList)
            {
                RoomManager room = node.GetComponentInChildren<RoomManager>();
                if (room.hasActiveInteracts || room.meshesEnabled) //If the player is currently in this room
                {
                    WaypointScript waypoint = node.GetComponent<WaypointScript>();

                    int randNum = Random.Range(0, waypoint.adjactentNodes.Count);
                    RoomManager rm = waypoint.adjactentNodes[randNum].GetComponentInChildren<RoomManager>();
                    if (!rm.meshesEnabled && !rm.litByFlashlight)
                    {
                        Vector3 adjacentRoom = waypoint.adjactentNodes[randNum].position;
                        transform.position = new Vector3(adjacentRoom.x, adjacentRoom.y + 1 - (WaypointManager.scale / 4), adjacentRoom.z);
                        break;
                    }
                    else
                    {
                        tempList.Remove(waypoint.transform);
                        MoveToNewRoom(tempList);
                        break;
                    }
                }
            }
        }
    }

    float UpdateTeleportTimer(int totalObjectives, int objectivesCollected, float multiplier)
    {
        float newTime;
        newTime = multiplier * ((totalObjectives + 1) - objectivesCollected);
        //Debug.Log(newTime);

        return newTime;
    }

    public void StartStunTimer()
    {
        StartCoroutine(StunTimer(stunTimer));
    }
    IEnumerator StunTimer(float stunTime)
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    public void TurnOnMesh()
    {
        GetComponent<MeshRenderer>().enabled = true;

        footprints.TurnOnMesh();
    }

    public void TurnOffMesh()
    {
        GetComponent<MeshRenderer>().enabled = false;

        footprints.TurnOffMesh();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("You died");
        }
        if (collision.transform.tag == "Interact")
        {
            InteractSetTrigger trigger = collision.transform.GetComponent<InteractSetTrigger>();
            if (currentRoom.hasActiveInteracts && trigger.state != InteractParent.State.Destroyed)
                trigger.state = InteractParent.State.Destroyed;
        }
    }
}
