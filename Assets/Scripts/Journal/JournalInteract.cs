﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalInteract : InteractParent {

    public Sprite sigilImage;
    Image journalUI;
    public string sigilWord;

    Player player;

    public bool collected;
    bool interacting = false;
    
    // Use this for initialization
	public override void Start () {
        base.Start();
        player = GameObject.FindObjectOfType<Player>();
        journalUI = GameObject.Find("journalUI").GetComponent<Image>();
        journalUI.GetComponent<CanvasGroup>().alpha = 0f;
        collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        //Display journal UI popup
        //Pause gameplay until user presses interact button, again
        //Should re-display journal UI every time they interact with this item
        if (!collected)
        {
            JournalController.IncrementJournal(sigilWord);
            collected = true;
        }

        journalUI.sprite = sigilImage;
        journalUI.GetComponent<CanvasGroup>().alpha = 1f;
        interacting = true;
        StartCoroutine(WaitForConfirm());
    }

    IEnumerator WaitForConfirm()
    {
        while (interacting)
        {
            yield return null;

            player.state = Player.State.Interact; //sloppy, but it works for now

            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("End interact");
                player.state = Player.State.Move;
                journalUI.GetComponent<CanvasGroup>().alpha = 0f;
                interacting = false;
            }
        }
    }
}
