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
            Vector3 velocity = Vector3.zero;
            Vector3 forward = player.transform.forward * WaypointManager.scale;
            Vector3 needPos = player.transform.position - forward * viewDist;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(needPos.x, player.transform.position.y + WaypointManager.scale * viewAngle + wpm.levels * WaypointManager.scale, needPos.z), ref velocity, turnSpeed);
            transform.LookAt(player.transform);
        }
    }
}
