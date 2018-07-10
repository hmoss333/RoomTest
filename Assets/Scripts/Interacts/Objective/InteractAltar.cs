using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAltar : InteractParent {

    Player player;
    Gregg killer;

    Transform basementExit;

    public float killerSpawnTime;
    bool killerTeleported = false;
    public GameObject weapon;

    public override void Start()
    {
        player = FindObjectOfType<Player>();
        basementExit = GameObject.Find("BasementExit").transform;
        base.Start();
    }

    public override void Interact()
    {
        if (!killerTeleported)
        {
            killer = GameObject.FindObjectOfType<Gregg>();

            player.weaponPrefab = weapon;
            GameManager.UpdateStep();

            base.Interact();
            //TO DO: killer fight logic here
            Debug.Log("Do secret ending here");
            StartCoroutine(TeleportKiller(killerSpawnTime));
        }
    }

    IEnumerator TeleportKiller(float teleportTime)
    {
        yield return new WaitForSeconds(teleportTime);
        killer.transform.position = new Vector3(basementExit.position.x, basementExit.position.y - (WaypointManager.scale / 8), basementExit.position.z);
        killerTeleported = true;
    }
}
