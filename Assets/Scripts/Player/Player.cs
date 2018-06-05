using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Movement Settings")]
    public float walkSpeed;
    public float dashSpeed;
    float speed;
    float xInput;
    float yInput;
    Vector3 dir;
    Vector3 lastDir;
    bool updatelastDir = false;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    public enum State { Move, Interact, Attack, Hide, Die}
    public State state;
    public Transform currentRoom;

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
    //bool attacking;

    [Header("Flashlight Settings")]
    public Flashlight flashlightPrefab;
    public static bool flashlightOn;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();

        speed = walkSpeed;
        flashlightOn = false;
        flashlightPrefab.GetComponent<Light>().enabled = flashlightOn;

        state = State.Move;
    }

    //
    //TO DO: Map inputs to controller
    //
    // Update is called once per frame
    void FixedUpdate () {
        if (state == State.Move)
        {
            //Control logic
            xInput = Input.GetAxisRaw("Horizontal");
            yInput = Input.GetAxisRaw("Vertical");

            dir = new Vector3(xInput, 0, yInput);
            dir = transform.TransformDirection(dir);
            dir *= speed;

            rb.velocity = dir;

            //Store last input direction
            if ((xInput != 0 || yInput != 0) && !updatelastDir)
            {
                lastDir = dir;
                updatelastDir = true;
            }
            else
            {
                updatelastDir = false;
            }

            //Set direction animations
            if (lastDir.x > 0)
            {
                direction = Direction.Right;
                animator.SetTrigger("Right");
            }
            else if (lastDir.x < 0)
            {
                direction = Direction.Left;
                animator.SetTrigger("Left");
            }
            else if (lastDir.z > 0)
            {
                direction = Direction.Up;
                animator.SetTrigger("Up");
            }
            else if (lastDir.z < 0)
            {
                direction = Direction.Down;
                animator.SetTrigger("Down");
            }

            //Speed controls [Left trigger?]
            if (Input.GetButton("Dash"))
            {
                speed = dashSpeed;
            }
            else
            {
                speed = walkSpeed;
            }

            speed *= WaypointManager.scale;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    void Update()
    {
        if (state == State.Move)
        {
            //Interact with objects [A button]
            if (Input.GetButtonDown("Interact"))
            {
                state = State.Interact;
                StartCoroutine(Interact(checkDist, checkTime));
            }

            //Attack with current weapon [B button]
            if (Input.GetButtonDown("Attack") && weaponPrefab != null)
            {
                state = State.Attack;
                StartCoroutine(Attack(weaponPrefab, direction, attackTime));
            }

            //Turn On/Off Flashlight [Right trigger]
            if (Input.GetButtonDown("Flashlight"))
            {
                Flashlight(flashlightPrefab);
            }

            //Enter Hide mode
            if (Input.GetButtonDown("Hide"))
            {
                flashlightOn = false;
                flashlightPrefab.GetComponent<Light>().enabled = flashlightOn;

                Hide();
            }
        }
        else if (state == State.Hide)
        {
            rb.velocity = Vector3.zero;

            if (Input.GetButtonDown("Hide"))
            {
                Hide();
            }

            if (Input.GetButtonDown("Attack") && weaponPrefab != null) //Attack out of hiding
            {
                state = State.Attack;
                StartCoroutine(Attack(weaponPrefab, direction, attackTime));
            }
        }
    }

    IEnumerator Interact(float interactDist, float interactTime)
    {
        foundHit = new RaycastHit();
        //Vector3 playerBase = new Vector3(transform.position.x, transform.position.y/2, transform.position.z);
        bool test = Physics.Raycast(transform.position/*playerBase*/, lastDir, out foundHit, interactDist, 1 << LayerMask.NameToLayer("Interact"));
        Debug.DrawRay(transform.position /*playerBase*/, lastDir, Color.green);

        if (test)
        {
            foundHit.transform.GetComponent<InteractParent>().Interact();
        }

        yield return new WaitForSeconds(interactTime);
        state = State.Move;
    }

    IEnumerator Attack(GameObject prefab, Direction currentDirection, float attackTime)
    {
        GameObject attackPrefab = null;

        switch (currentDirection)
        {
            case Direction.Up:
                attackPrefab = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), Quaternion.identity);
                attackPrefab.transform.parent = this.transform;
                break;
            case Direction.Down:
                attackPrefab = Instantiate(prefab, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity);
                attackPrefab.transform.parent = this.transform;
                break;
            case Direction.Left:
                attackPrefab = Instantiate(prefab, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), Quaternion.identity);
                attackPrefab.transform.parent = this.transform;
                break;
            case Direction.Right:
                attackPrefab = Instantiate(prefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Quaternion.identity);
                attackPrefab.transform.parent = this.transform;
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }

        yield return new WaitForSeconds(attackTime);
        Destroy(attackPrefab);
        state = State.Move;
    }

    void Flashlight(Flashlight prefab)
    {
        //Keeps triggering twice

        if (flashlightOn)
        {
            flashlightOn = false;
        }
        else
        {
            flashlightOn = true;
        }

        prefab.GetComponent<Light>().enabled = flashlightOn;
    }

    void Hide()
    {
        if (state != State.Hide)
            state = State.Hide;
        else
            state = State.Move;
    }
}
