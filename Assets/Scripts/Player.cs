using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    public float checkDist;

    float xInput;
    float yInput;

    Vector3 dir;
    Vector3 lastDir;
    bool updatelastDir = false;

    Rigidbody rb;
    Animator animator;

    RaycastHit foundHit;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
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
            animator.SetTrigger("Right");
        else if (lastDir.x < 0)
            animator.SetTrigger("Left");
        else if (lastDir.z > 0)
            animator.SetTrigger("Up");
        else if (lastDir.z < 0)
            animator.SetTrigger("Down");

        //Interact with objects
        if (Input.GetButtonDown("Jump"))
        {
            Interact();
        }
    }

    void Interact()
    {
        foundHit = new RaycastHit();
        Vector3 playerBase = new Vector3(transform.position.x, transform.position.y/2, transform.position.z);
        bool test = Physics.Raycast(playerBase, lastDir, out foundHit, checkDist, 1 << LayerMask.NameToLayer("Interact"));
        Debug.DrawRay(playerBase, lastDir, Color.green);

        if (test)
        {
            foundHit.transform.GetComponent<InteractParent>().Interact();
        }
    }
}
