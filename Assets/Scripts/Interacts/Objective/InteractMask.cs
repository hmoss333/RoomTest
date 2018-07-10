using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMask : InteractParent
{
    UIManager uim;

	// Use this for initialization
	void Start () {
        uim = GameObject.FindObjectOfType<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        base.Interact();
    }

    IEnumerator FadetoBlack()
    {
        yield return new WaitForSeconds(10);
        GameManager.gameState = GameManager.GameState.Win;
    }
}
