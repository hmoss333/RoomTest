using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalInteract : InteractParent {

    public Sprite sigilImage;
    public Sprite journalBackground;
    Image journalUI;
    Image sigilUI;
    //Text sigilText;
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
        //sigilText = GameObject.Find("sigilUI").GetComponent<Text>();
        journalUI.GetComponent<CanvasGroup>().alpha = 0f;
        //collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        //tc.textBoxBackground.GetComponent<CanvasGroup>().alpha = 0f;

        sigilUI.sprite = sigilImage;
        journalUI.sprite = journalBackground;
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
                JournalController.IncrementJournal(sigilWord);

                //tc.textBoxBackground.GetComponent<CanvasGroup>().alpha = 1f;
                //tc.StartCoroutine(tc.FadeOutText(tc.textBoxBackground, tc.textDelay));

                base.Interact();
                this.gameObject.SetActive(false);
            }
        }
    }
}
