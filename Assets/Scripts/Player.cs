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

    RaycastHit foundHit;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        //itemCount = PlayerPrefs.GetInt("itemCount");
    }
	
	// Update is called once per frame
	void Update () {
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

        //Interact with objects
        if (Input.GetButtonDown("Jump"))
        {
            Interact();
        }
    }

    void Interact()
    {
        foundHit = new RaycastHit();
        bool test = Physics.Raycast(transform.position, lastDir, out foundHit, checkDist, 1 << LayerMask.NameToLayer("Interact"));
        Debug.DrawRay(transform.position, lastDir, Color.green);

        if (test)
        {
            foundHit.transform.GetComponent<InteractParent>().Interact();
        }
    }
}
