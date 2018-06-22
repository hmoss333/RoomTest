﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractDoor : InteractParent {

    public DoorManager doorManager;
    public Sprite sigilImage;
    Image journalUI;
    public string sigilWord;
    bool interacting = false;
    //public bool unlocked = false;

    JournalController jc;
    Player player;

    public override void Start()
    {
        base.Start();
        jc = GameObject.FindObjectOfType<JournalController>();
        player = GameObject.FindObjectOfType<Player>();
        journalUI = GameObject.Find("journalUI").GetComponent<Image>();
        journalUI.GetComponent<CanvasGroup>().alpha = 0f;
    }

    public override void Interact()
    {
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
                if (jc.foundSigils.Contains(sigilWord))
                {
                    doorManager.roomLocked = false;
                    doorManager.DestroyDoors(doorManager.connectedDoors);
                    text = "The door unlocked when I used the symbol from the journal";
                }
                else
                {
                    text = "I don't see anywhere for a key, just a weird symbol...";
                }
                journalUI.GetComponent<CanvasGroup>().alpha = 0f;
                player.state = Player.State.Move;
                interacting = false;
                base.Interact();
            }
        }
    }
}
