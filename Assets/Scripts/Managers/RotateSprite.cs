using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour {

    Player player;
    Camera mainCam;
    SpriteRenderer sprite;

    float spriteHeight = 0;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        mainCam = Camera.main;
        sprite = GetComponentInChildren<SpriteRenderer>();

        CalculateSpriteHeight(mainCam, this.transform);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        UpdateSprite();
    }

    void UpdateSprite()
    {
        //Rotation Logic
        sprite.transform.rotation = player.transform.rotation; //works on the y axis, but not X or Z
        //sprite.transform.LookAt(mainCam.transform); //swaps left and right animations
        //sprite.transform.rotation = Quaternion.RotateTowards(sprite.transform.rotation, mainCam.transform.rotation, 90);

        //Scaling/Positioning Logic
        sprite.transform.localScale = new Vector3(sprite.transform.localScale.x, spriteHeight, sprite.transform.localScale.z);
        sprite.transform.position = new Vector3(sprite.transform.position.x, transform.position.y + 0.25f, sprite.transform.position.z);
    }

    void CalculateSpriteHeight(Camera cam, Transform parent)
    {
        Vector3 test = cam.transform.rotation.eulerAngles;

        spriteHeight = Mathf.Sqrt(Mathf.Pow(test.x / 2, 2) + Mathf.Pow(parent.lossyScale.y * (WaypointManager.scale/2), 2));
        //Debug.Log(test.x / 2 + " -> " + Mathf.Pow(test.x / 2, 2));
        //Debug.Log(parent.lossyScale.y * (WaypointManager.scale / 2) + " -> " + Mathf.Pow(parent.lossyScale.y * (WaypointManager.scale / 2), 2));
        //Debug.Log(spriteHeight);
    }
}
