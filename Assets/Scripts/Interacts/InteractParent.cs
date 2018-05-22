using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractParent : MonoBehaviour {

	public enum State { Off, Fire, Wet, Electric, Trap, Destroyed, Disabled}
    public State state;
    
    // Use this for initialization
	void Start () {
        state = State.Off;
	}
	
	// Update is called once per frame
	void Update () {

	}

    public virtual void Interact()
    {
        //TO DO: general interact logic can go here
    }
}
