using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWeapon : InteractObject {

    Player player;

    public GameObject weapon;

    // Use this for initialization
    public override void Start () {
        player = FindObjectOfType<Player>();
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        player.weaponPrefab = weapon;
        this.gameObject.SetActive(false);
        base.Interact();
    }
}
