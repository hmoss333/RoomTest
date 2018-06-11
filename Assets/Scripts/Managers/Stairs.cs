using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

    public float waitTime;
    public Transform exitTransform;
    public static bool inUse = false;

    WaypointManager wpm;

    // Use this for initialization
	void Start () {
        wpm = GameObject.FindObjectOfType<WaypointManager>();

        exitTransform = GetExit(wpm.waypointNodes);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    Transform GetExit(List<Transform> roomList)
    {
        foreach (Transform room in roomList)
        {
            if (room != this.GetComponentInParent<WaypointScript>().transform)
            {
                if (room.GetComponent<WaypointScript>().type == WaypointScript.Type.stairs)
                {
                    return room.GetComponentInChildren<Stairs>().transform;
                }
            }
        }

        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inUse)
        {
            other.gameObject.transform.position = new Vector3(exitTransform.position.x, exitTransform.position.y + 1 - (WaypointManager.scale / 4), exitTransform.position.z);
            StartCoroutine(UpdatePosition(waitTime));
        }
    }

    IEnumerator UpdatePosition(float time)
    {
        inUse = true;
        yield return new WaitForSeconds(time);
        inUse = false;
    }
}
