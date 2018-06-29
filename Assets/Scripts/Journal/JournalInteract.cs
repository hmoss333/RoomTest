using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalInteract : InteractParent {

    public Sprite sigilImage;
    Image journalUI;
    Image sigilUI;
    public string sigilWord;

    Player player;

    //public bool collected;
    bool interacting = false;
    
    // Use this for initialization
	public override void Start () {
        base.Start();
        player = GameObject.FindObjectOfType<Player>();
        journalUI = GameObject.Find("journalUI").GetComponent<Image>();
        sigilUI = GameObject.Find("sigilUI").GetComponent<Image>();
        journalUI.GetComponent<CanvasGroup>().alpha = 0f;
        //collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        //Display journal UI popup
        //Pause gameplay until user presses interact button, again
        //Should re-display journal UI every time they interact with this item
        sigilUI.GetComponent<Image>().sprite = sigilImage;
        journalUI.GetComponent<CanvasGroup>().alpha = 1f;
        interacting = true;
        StartCoroutine(WaitForConfirm());
    }

    IEnumerator WaitForConfirm()
    {
        while (interacting)
        {
            yield return null;

            player.state = Player.State.Interact; //sloppy, but it works for now; calling every frame to overwrite Player class

            if (Input.GetButtonDown("Interact"))
            {
                player.state = Player.State.Move;
                journalUI.GetComponent<CanvasGroup>().alpha = 0f;
                interacting = false;
                //if (!collected)
                //{
                    JournalController.IncrementJournal(sigilWord);
                    base.Interact();
                //    collected = true;
                //}
                this.gameObject.SetActive(false);
            }
        }
    }
}
