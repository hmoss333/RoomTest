using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoom : MonoBehaviour {

    public MeshRenderer[] roomMeshes;
    public bool meshesEnabled;

    // Use this for initialization
	void Start () {
        roomMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in roomMeshes)
        {
            mesh.enabled = false;
        }

        meshesEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && meshesEnabled == false)
        {
            foreach (MeshRenderer mesh in roomMeshes)
            {
                mesh.enabled = true;
            }

            meshesEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("left room");
            //renderer.enabled = true;

            foreach (MeshRenderer mesh in roomMeshes)
            {
                mesh.enabled = false;
            }

            meshesEnabled = false;
        }
    }
}
