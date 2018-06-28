using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDoor : MonoBehaviour {

    Transform basementExit;

    // Use this for initialization
	void Start () {
        basementExit = GameObject.Find("BasementExit").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.step = 5;
            Vector3 targetPos = new Vector3(basementExit.position.x, basementExit.position.y - (WaypointManager.scale / 8), basementExit.position.z);

            other.transform.position = targetPos;
        }
    }
}
