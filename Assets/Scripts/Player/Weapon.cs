using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //Destroy interacts
        //if (other.tag == "Interact")
        //{
        //    Debug.Log("Hit an interact object"); //this requires a rigidbody in order to register
        //}

        //Stun the killer
        if (other.tag == "Killer")
        {
            //Debug.Log("Hit the killer");
            Gregg killer = other.GetComponent<Gregg>();
            if (!killer.stunned)
                killer.StartStunTimer();
        }
    }
}
