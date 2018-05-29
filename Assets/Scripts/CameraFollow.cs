using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Player player;

    public RaycastHit[] hits = null;
    Color col;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        transform.position = player.transform.position;
    }

    private void Update()
	{
        //Set all objects to be visible
        if (hits != null)
        {
            foreach (RaycastHit hit in hits)
            {
                Renderer r = hit.collider.GetComponent<Renderer>();
                if (r)
                {
                    col = r.material.color;
                    col.a = 1.0f;
                    r.material.color = col;
                }
            }
        }

        hits = Physics.RaycastAll(this.transform.position, (player.transform.position - this.transform.position), Vector3.Distance(this.transform.position, player.transform.position));

        //Hide objects between the player and the camera
        if (hits != null)
        {
            foreach (RaycastHit hit in hits)
            {
                Renderer r = hit.collider.GetComponent<Renderer>();
                if (r)
                {
                    col = r.material.color;
                    col.a = 0.5f;
                    r.material.color = col;
                }
            }
        }

        Vector3 velocity = Vector3.zero;
        Vector3 forward = player.transform.forward * WaypointManager.scale;// 5f;
        Vector3 needPos = player.transform.position - forward;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(needPos.x, WaypointManager.scale * 0.75f, needPos.z), ref velocity, 0.02f);
        transform.LookAt(player.transform);
    }
}
