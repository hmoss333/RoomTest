using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public bool isMachette = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Killer")
        {
            Gregg killer = other.GetComponent<Gregg>();
            //Debug.Log("Hit the killer");
            if (!killer.stunned)
                killer.StartStunTimer();

            if (isMachette)
            {
                killer.health--;
            }
        }
    }
}
