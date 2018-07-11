using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMask : InteractParent
{
    UIManager uim;

	// Use this for initialization
	public override void Start () {
        base.Start();
        uim = GameObject.FindObjectOfType<UIManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        GameManager.gameState = GameManager.GameState.Interacting;
        base.Interact();
        StartCoroutine(FadetoBlack());
    }

    IEnumerator FadetoBlack()
    {
        Debug.Log("Put screen fade logic here");
        yield return new WaitForSeconds(10);
        GameManager.gameState = GameManager.GameState.Win;
        uim.WinMenu();
        Destroy(this.gameObject);
    }
}
