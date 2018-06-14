using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractParent : MonoBehaviour {

    public enum State { Off, Fire, Wet, Electric, Trap, Destroyed, Disabled}
    public State state;

    public TextController tc;
    public string text;

    
    // Use this for initialization
	public virtual void Start () {
        state = State.Off;
        tc = GameObject.FindObjectOfType<TextController>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public virtual void Interact()
    {
        //TO DO: general interact logic can go here

        TextController.textToDisplay = text;
        tc.DisplayText();
    }
}
