using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour {

    Player player;
    SpriteRenderer sprite;
    
    // Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        sprite = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (sprite.transform.rotation != player.transform.rotation)
        {
            UpdateSpriteRotation();
        }
	}

    void UpdateSpriteRotation()
    {
        sprite.transform.rotation = player.transform.rotation;
    }
}
