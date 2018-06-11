using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour {

    public GameObject doorPrefab;
    public bool roomLocked = true;

    HideRoom roomScript;
    public LayerMask layerMask;
    
    // Use this for initialization
	void Start () {
        roomScript = GetComponent<HideRoom>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((roomScript.meshesEnabled || roomScript.litByFlashlight) && roomLocked == true)
        {
            HideRoomObjects(roomScript.roomMeshes);
        }
        else if (!roomLocked)
        {
            ShowRoomObjects(roomScript.roomMeshes);
        }
	}

    void HideRoomObjects(MeshRenderer[] roomMeshes)
    {
        Debug.Log("Hiding...");
        foreach (MeshRenderer mesh in roomMeshes)
        {
            Debug.Log(mesh.name);
            if (mesh.gameObject.layer == layerMask)
            {
                mesh.gameObject.GetComponent<Material>().color = Color.black;
            }
        }
    }

    void ShowRoomObjects(MeshRenderer[] roomMeshes)
    {
        foreach (MeshRenderer mesh in roomMeshes)
        {
            if (mesh.gameObject.layer == layerMask)
            {
                mesh.gameObject.GetComponent<Material>().color = Color.white;
            }
        }
    }
}
