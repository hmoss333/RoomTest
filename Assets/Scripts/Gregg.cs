using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gregg : MonoBehaviour {

    [Header("Movement Settings")]
    public float walkSpeed;
    public float hurtSpeed;
    float speed;
    float xInput;
    float yInput;
    Vector3 dir;
    Vector3 lastDir;
    bool updatelastDir = false;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    public enum State { Move, Interact, Attack, Stun, Die }
    public State state;
    public Transform currentRoom;
    public Transform nextRoom;
    public bool moveToNextRoom;

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

        characterRenderer = GetComponentsInChildren<SpriteRenderer>();
        //foreach (SpriteRenderer mesh in characterRenderer)
        //    mesh.enabled = false;

        //nextRoom = FindNextRoom(currentRoom);
        StartCoroutine(MoveTimer(5f));
        StartCoroutine(DisapearTimer(45f));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (moveToNextRoom)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(nextRoom.position.x, 1 - (WaypointManager.scale / 4), nextRoom.position.z), speed * Time.deltaTime);
        }
	}

    IEnumerator MoveTimer(float waitTime)
    {
        //moveToNextRoom = false;

        yield return new WaitForSeconds(waitTime);

        moveToNextRoom = true;

        //while (transform.position != nextRoom.position)
        //    yield return null;

        MoveTimer(waitTime);
    }

    IEnumerator DisapearTimer(float deathTime)
    {
        yield return new WaitForSeconds(deathTime);
        //TO DO: send message to GameManager to start respawn timer
        Destroy(this.gameObject);
    }

    public void TurnOnMesh()
    {
        foreach (SpriteRenderer mesh in characterRenderer)
            mesh.enabled = true;
    }

    public void TurnOffMesh()
    {
        foreach (SpriteRenderer mesh in characterRenderer)
            mesh.enabled = false;
    }

    public void SetNextRoom(Transform target)
    {
        nextRoom = target;
    }
}
