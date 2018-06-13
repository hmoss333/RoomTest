using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Player player;

    public RaycastHit[] hits = null;
    public RaycastHit[] wallhits = null;
    Color col;

    public float viewAngle;
    [Range(0.01f, 0.1f)]
    public float turnSpeed;
    public float viewDist;
    public LayerMask layersToFade;
    public LayerMask outerWallLayers;

    WaypointManager wpm;

    // Use this for initialization
    void Start()
    {
        Camera mainCam = Camera.main;
        //mainCam.transparencySortMode = TransparencySortMode.Orthographic;
        mainCam.opaqueSortMode = UnityEngine.Rendering.OpaqueSortMode.FrontToBack;
        player = GameObject.FindObjectOfType<Player>();
        transform.position = player.transform.position;

        wpm = GameObject.FindObjectOfType<WaypointManager>();
    }

    private void FixedUpdate()
	{
        if (!player)
            player = GameObject.FindObjectOfType<Player>();

        else
        {
            //Set all objects to be visible
            if (hits != null)
                ShowAll(hits);
            if (wallhits != null)
                ShowAll(wallhits);

            hits = Physics.RaycastAll(this.transform.position, (player.transform.position - this.transform.position), Vector3.Distance(this.transform.position, player.transform.position), layersToFade);
            wallhits = Physics.RaycastAll(this.transform.position, (player.transform.position - this.transform.position), Vector3.Distance(this.transform.position, player.transform.position), outerWallLayers);

            //Hide objects between the player and the camera
            if (hits != null)
                HideHits(hits, 0.5f);
            if (wallhits != null)
                HideHits(wallhits, 0.0f);

            Vector3 velocity = Vector3.zero;
            Vector3 forward = player.transform.forward * WaypointManager.scale;// 5f;
            Vector3 needPos = player.transform.position - forward * viewDist;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(needPos.x, player.transform.position.y + WaypointManager.scale * viewAngle + wpm.levels * WaypointManager.scale, needPos.z), ref velocity, turnSpeed);
            transform.LookAt(player.transform);
        }
    }

    void ShowAll(RaycastHit[] hits)
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

    void HideHits(RaycastHit[] hits, float alpha)
    {
        foreach (RaycastHit hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r)
            {
                col = r.material.color;
                col.a = alpha;
                r.material.color = col;
            }
        }
    }
}
