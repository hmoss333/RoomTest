using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Player player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        transform.position = player.transform.position;
    }

    private void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 forward = player.transform.forward * WaypointManager.scale;// 5f;
        Vector3 needPos = player.transform.position - forward;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(needPos.x, WaypointManager.scale, needPos.z),
                                                ref velocity, 0.02f);
        transform.LookAt(player.transform);
    }
}
