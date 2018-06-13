using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Movement Settings")]
    public float walkSpeed;
    public float dashSpeed;
    float speed;
    public float xInput;
    public float yInput;
    Vector3 dir;
    //[HideInInspector]
    public Vector3 lastDir;
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

        direction = Direction.Down;
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

            //Store last input direction; may not be necessary
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
            if (xInput > 0)
            {
                direction = Direction.Right;
            }
            else if (xInput < 0)
            {
                direction = Direction.Left;
            }
            else if (yInput > 0)
            {
                direction = Direction.Up;
            }
            else if (yInput < 0)
            {
                direction = Direction.Down;
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
            //If not in the move state, player cannot move
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
                StartCoroutine(Attack(weaponPrefab, lastDir, attackTime));
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
                StartCoroutine(Attack(weaponPrefab, lastDir, attackTime));
            }
        }

        if (Input.GetButtonDown("CamLeft"))
        {
            RotateCamLeft(direction);
        }
        else if (Input.GetButtonDown("CamRight"))
        {
            RotateCamRight(direction);
        }

        SetAnimation(direction); //Will cause issues with memory usage down the line
    }

    IEnumerator Interact(float interactDist, float interactTime)
    {
        foundHit = new RaycastHit();
        bool test = Physics.Raycast(transform.position, lastDir, out foundHit, interactDist, 1 << LayerMask.NameToLayer("Interact"));
        Debug.DrawRay(transform.position, lastDir, Color.green);

        if (test)
        {
            foundHit.transform.GetComponent<InteractParent>().Interact();
        }

        yield return new WaitForSeconds(interactTime);
        state = State.Move;
    }

    IEnumerator Attack(GameObject prefab, Vector3 direction, float attackTime)
    {
        GameObject attackPrefab = null;
        Vector3 attackDir = transform.position + (direction / (WaypointManager.scale/2));

        attackPrefab = Instantiate(prefab, attackDir, Quaternion.identity);

        yield return new WaitForSeconds(attackTime);
        Destroy(attackPrefab);
        state = State.Move;
    }

    void Flashlight(Flashlight prefab)
    {
        if (flashlightOn)
            flashlightOn = false;
        else
            flashlightOn = true;

        prefab.GetComponent<Light>().enabled = flashlightOn;
    }

    void Hide()
    {
        if (state != State.Hide)
            state = State.Hide;
        else
            state = State.Move;
    }

    void RotateCamLeft(Direction currentDirection)
    {
        transform.Rotate(0, 90, 0, Space.World);

        switch (currentDirection)
        {
            case Direction.Up:
                direction = Direction.Left;
                break;
            case Direction.Right:
                direction = Direction.Up;
                break;
            case Direction.Down:
                direction = Direction.Right;
                break;
            case Direction.Left:
                direction = Direction.Down;
                break;
            default:
                break;
        }
    }

    void RotateCamRight(Direction currentDirection)
    {
        transform.Rotate(0, -90, 0, Space.World);

        switch (currentDirection)
        {
            case Direction.Up:
                direction = Direction.Right;
                //animator.SetTrigger("Right");
                break;
            case Direction.Left:
                direction = Direction.Up;
                //animator.SetTrigger("Up");
                break;
            case Direction.Down:
                direction = Direction.Left;
                //animator.SetTrigger("Left");
                break;
            case Direction.Right:
                direction = Direction.Down;
                //animator.SetTrigger("Down");
                break;
            default:
                break;
        }
    }

    void SetAnimation(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                animator.SetTrigger("Up");
                break;
            case Direction.Left:
                animator.SetTrigger("Left");
                break;
            case Direction.Down:
                animator.SetTrigger("Down");
                break;
            case Direction.Right:
                animator.SetTrigger("Right");
                break;
            default:
                break;
        }
    }
}
