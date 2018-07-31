using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gregg : MonoBehaviour {

    [Header("Movement Settings")]
    public float testNum;
    public float speed;
    float teleportTime;
    public float footprintSpacing = 2.0f; // distance between each footprint
    Vector3 lastPos;
    public bool stunned = false;
    public float stunTimer;
    public int health = 3;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    public float XOff;
    public float YOff;

    [Header("Interact Settings")]
    public float checkDist;
    public float checkTime;
    RaycastHit foundHit;
    public LayerMask layerMask;
    bool playerHiding = false;

    [Header("Controls References")]
    Rigidbody rb;
    Animator animator;

    [Header("Attack Settings")]
    public GameObject weaponPrefab;
    public float attackTime;
    public float attackDist;
    bool attacking = false;

    SpriteRenderer[] characterRenderer;

    WaypointManager wpm;
    GameManager gm;
    Player player;
    Footprints footprints;
    public RoomManager currentRoom;
    Vector3 targetRoomPos;
    public GameObject maskPrefab;

    // Use this for initialization
    void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();
        gm = GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindObjectOfType<Player>();
        animator = gameObject.GetComponentInChildren<Animator>();

        attackDist = WaypointManager.scale / 8;
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
        if (GameManager.gameState == GameManager.GameState.Playing && health > 0)
        {
            if (!playerHiding)
            {
                if (!stunned && !attacking)
                {
                    if (currentRoom && currentRoom.hasActiveInteracts && (!currentRoom.meshesEnabled || !currentRoom.litByFlashlight))
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentRoom.interacts[Random.Range(0, currentRoom.interacts.Length)].transform.position.x, transform.position.y, currentRoom.interacts[Random.Range(0, currentRoom.interacts.Length)].transform.position.z), speed * Time.deltaTime);
                    else
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z), speed * Time.deltaTime);
                }

                if (!attacking && Vector3.Distance(transform.position, player.transform.position) <= attackDist)
                    StartCoroutine(Attack(attackTime));
            }
            else
            {
                if (!stunned && !attacking)
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetRoomPos.x, transform.position.y, targetRoomPos.z), speed * Time.deltaTime);
            }

            //Footprint logic
            float distFromLastFootprint = (lastPos - transform.position).sqrMagnitude;
            if (distFromLastFootprint > footprintSpacing * footprintSpacing)
            {
                footprints.AddFootprint(transform.position, transform.forward, transform.right, Random.Range(0, 4));

                lastPos = transform.position;
            }

            if (footprints.transform.position.y != currentRoom.transform.position.y - WaypointManager.scale)
                footprints.transform.position = new Vector3(0, currentRoom.transform.position.y - WaypointManager.scale, 0);

            //Hiding logic
            if (Physics.Linecast(transform.position, player.transform.position, layerMask) && player.state == Player.State.Hide) //if there is something between the killer and the player
            {
                if (!playerHiding)
                {
                    playerHiding = true;
                    targetRoomPos = FindNewRoom(currentRoom); //should only be called once here
                }
            }
            else
                playerHiding = false;

            //Animating Logic
            float horizOff = Mathf.Abs(player.transform.position.x - transform.position.x);
            float vertOff = Mathf.Abs(player.transform.position.z - transform.position.z);

            if (vertOff < Mathf.Abs(XOff) && horizOff > Mathf.Abs(YOff))
            {
                if (transform.position.x > player.transform.position.x)
                    direction = Direction.Left;
                if (transform.position.x < player.transform.position.x)
                    direction = Direction.Right;
            }
            else if (vertOff > Mathf.Abs(XOff) && horizOff < Mathf.Abs(YOff))
            {
                if (transform.position.z < player.transform.position.z)
                    direction = Direction.Up;
                if (transform.position.z > player.transform.position.z)
                    direction = Direction.Down;
            }

            CheckRotation(direction, player.transform.rotation.eulerAngles);
            animator.SetTrigger(direction.ToString());
        }

        if (health <= 0)
        {
            Debug.Log("Play death animation");
            if (GameManager.step == 6)
            {
                Debug.Log("Drop Mask object");
                maskPrefab = Instantiate(maskPrefab, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }
    }

    void CheckRotation(Direction currentDirection, Vector3 playerRotation)
    {
        if (playerRotation.y != 0)
        {
            switch (currentDirection)
            {
                case Direction.Up:
                    if (playerRotation.y == 90)
                        direction = Direction.Left;
                    else if (playerRotation.y == -90 || playerRotation.y == 270)
                        direction = Direction.Right;
                    else
                        direction = Direction.Down;
                    break;
                case Direction.Right:
                    if (playerRotation.y == 90)
                        direction = Direction.Up;
                    else if (playerRotation.y == -90 || playerRotation.y == 270)
                        direction = Direction.Down;
                    else
                        direction = Direction.Left;
                    break;
                case Direction.Down:
                    if (playerRotation.y == 90)
                        direction = Direction.Right;
                    else if (playerRotation.y == -90 || playerRotation.y == 270)
                        direction = Direction.Left;
                    else
                        direction = Direction.Up;
                    break;
                case Direction.Left:
                    if (playerRotation.y == 90)
                        direction = Direction.Down;
                    else if (playerRotation.y == -90 || playerRotation.y == 270)
                        direction = Direction.Up;
                    else
                        direction = Direction.Right;
                    break;
                default:
                    break;
            }
        }
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
        if (!GetComponentInChildren<SpriteRenderer>().enabled) //!GetComponent<MeshRenderer>().enabled) //If killer is currently not visible
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
        DoorManager dm = currentRoom.GetComponent<DoorManager>();
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        if (dm)
        {
            if (!dm.roomLocked)
            {
                foreach (SpriteRenderer sprite in sprites)
                    sprite.enabled = true;

                footprints.TurnOnMesh();
            }
            else
            {
                foreach (SpriteRenderer sprite in sprites)
                    sprite.enabled = false;

                footprints.TurnOffMesh();
            }
        }
        else
        {
            foreach (SpriteRenderer sprite in sprites)
                sprite.enabled = true;

            footprints.TurnOnMesh();
        }
    }

    public void TurnOffMesh()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = false;

        footprints.TurnOffMesh();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Interact")
        {
            InteractSetTrigger trigger = collision.transform.GetComponent<InteractSetTrigger>();
            if (currentRoom.hasActiveInteracts && trigger.state != InteractParent.State.Destroyed)
                trigger.state = InteractParent.State.Destroyed;
        }
    }

    IEnumerator Attack(float attackTime)
    {
        attacking = true;
        Debug.Log("Start attack");
        yield return new WaitForSeconds(attackTime);
        //attack logic here
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDist && !stunned)
        {
            Debug.Log("Hit player");
            GameManager.gameState = GameManager.GameState.Lose;
        }
        else
        {
            Debug.Log("Missed player");
        }
        attacking = false;
    }

    Vector3 FindNewRoom(RoomManager currentRoomPos)
    {
        Vector3 tempPos;
        tempPos = currentRoomPos.GetComponentInParent<WaypointScript>().adjactentNodes[Random.Range(0, currentRoomPos.GetComponentInParent<WaypointScript>().adjactentNodes.Count)].transform.position;

        return tempPos;
    }
}
