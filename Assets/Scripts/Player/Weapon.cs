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
            if (!isMachette)
            {
                if (!killer.stunned)
                    killer.StartStunTimer();
            }
            else
            {
                //GameManager.gameState = GameManager.GameState.Win;
                //GameManager.UpdateStep();

                if (!killer.stunned)
                    killer.StartStunTimer();

                killer.health--;
            }
        }
    }
}
