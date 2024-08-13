using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    [Header("Movement Settings")]
    float speed;
    public float walkSpeed;
    public float dashSpeed;
    float dashTime;
    bool tired = false;
    //float xInput;
    //float yInput;
    //float xInputMove;
    //float yInputMove;
    Vector3 dir;
    Vector3 moveDir;
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
    public TextController tc;

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
        tc = GameObject.FindObjectOfType<TextController>();
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();

        speed = walkSpeed;
        flashlightOn = true;
        flashlightPrefab.GetComponent<Light>().enabled = flashlightOn;
        updatelastDir = false;

        state = State.Move;
        direction = Direction.Down;
        lastDir = new Vector3(0, 0, -1.0f);
    }

    //
    //TO DO: Map inputs to controller
    //
    // Update is called once per frame
    void FixedUpdate () {
        if (GameManager.gameState == GameManager.GameState.Playing && state == State.Move)
        {
            Vector2 moveInput = InputManager.instance.inputController.Player.Move.ReadValue<Vector2>();
            Vector2 flashlightInput = InputManager.instance.inputController.Player.Flashlight.ReadValue<Vector2>();

            //Control logic
            //xInput = flashlightInput.x;//Input.GetAxisRaw("Horizontal");
            //yInput = flashlightInput.y;//Input.GetAxisRaw("Vertical");
            //xInputMove = playerInput.x;//Input.GetAxisRaw("HorizontalMove");
            //yInputMove = playerInput.y;//Input.GetAxisRaw("VerticalMove");

            //Look direction controls
            dir = new Vector3(flashlightInput.x, 0, flashlightInput.y);
            dir = transform.TransformDirection(dir);
            dir *= speed;

            //Move direction controls
            moveDir = new Vector3(moveInput.x, 0, moveInput.y);
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;

            //rb.velocity = dir;
            rb.velocity = moveDir;

            //Store last input direction
            if ((flashlightInput.x != 0 || flashlightInput.y != 0) && !updatelastDir)
            {
                lastDir = dir;
                updatelastDir = true;
            }
            else
            {
                updatelastDir = false;
            }

            //Set look direction animations
            if (flashlightInput.x > 0)
            {
                direction = Direction.Right;
            }
            else if (flashlightInput.x < 0)
            {
                direction = Direction.Left;
            }
            else if (flashlightInput.y > 0)
            {
                direction = Direction.Up;
            }
            else if (flashlightInput.y < 0)
            {
                direction = Direction.Down;
            }

            //Speed controls [Left trigger?]
            if (InputManager.instance.inputController.Player.Dash.IsPressed() && !tired)
            {
                speed = dashSpeed;
                if (dashTime > 0.0f)
                {
                    dashTime -= Time.deltaTime;
                }
                else
                    tired = true;
            }
            else
            {
                speed = walkSpeed;
                if (!InputManager.instance.inputController.Player.Dash.IsPressed() && dashTime < 1.0f)
                {
                    dashTime += Time.deltaTime;
                    tired = false;
                }
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
        if (GameManager.gameState == GameManager.GameState.Playing)
        {
            if (state == State.Move)
            {
                //Interact with objects [A button]
                if (InputManager.instance.inputController.Player.Interact.triggered)
                {
                    state = State.Interact;
                    StartCoroutine(Interact(checkDist, checkTime));
                }

                //Attack with current weapon [B button]
                if (InputManager.instance.inputController.Player.Melee.triggered && weaponPrefab != null)
                {
                    state = State.Attack;
                    StartCoroutine(Attack(weaponPrefab, lastDir, attackTime));
                }

                //Turn On/Off Flashlight [Right trigger]
                if (InputManager.instance.inputController.Player.FlashlightToggle.triggered)
                {
                    Flashlight(flashlightPrefab);
                }
            }

            if (InputManager.instance.inputController.Player.CamLeft.triggered)
            {
                RotateCamLeft(direction);
            }
            else if (InputManager.instance.inputController.Player.CamRight.triggered)
            {
                RotateCamRight(direction);
            }

            SetAnimation(direction); //Should really not be calling this from Update
        }
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
        Vector3 attackDir = transform.position + (direction / (WaypointManager.scale / 3));

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

    //void Hide()
    //{
    //    if (state != State.Hide)
    //        state = State.Hide;
    //    else
    //        state = State.Move;
    //}

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
                break;
            case Direction.Left:
                direction = Direction.Up;
                break;
            case Direction.Down:
                direction = Direction.Left;
                break;
            case Direction.Right:
                direction = Direction.Down;
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
