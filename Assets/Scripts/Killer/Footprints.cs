using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footprints : MonoBehaviour {

    public int maxFootprints = 256; // Maximum number of footprints total handled by one instance of the script.
    public Vector2 footprintSize = new Vector2( 0.4f, 0.8f ); // The size of the footprint. Should match the size of the footprint that it is used for. In meters.
    public float footprintSpacing = 0.3f; // the offset for the left or right footprint. In meters.
    public float groundOffset = 0.02f;    // The distance the footprints are places above the surface it is placed upon. In meters.
    public LayerMask terrainLayer; // the layer of the terrain, so the footprint raycast is only hitting the terrain.
    //public float footprintSpacing = 2.0f; // distance between each footprint

    private Mesh mesh;
 
     private Vector3[] vertices;
     private Vector3[] normals;
     private Vector2[] uvs;
     private int[] triangles;
 
     private int footprintCount = 0;
 
     private bool isLeft = false;
 
 
     // Initializes the array holding the footprint sections.
    void Awake()
    {
        // - Initialize Arrays -

        vertices = new Vector3[maxFootprints * 4];
        normals = new Vector3[maxFootprints * 4];
        uvs = new Vector2[maxFootprints * 4];
        triangles = new int[maxFootprints * 6];

        // - Initialize Mesh -

        if (GetComponent<MeshFilter>().mesh == null)
        {
            GetComponent<MeshFilter>().mesh = new Mesh();
        }

        mesh = GetComponent<MeshFilter>().mesh;

        mesh.name = "Footprints_Mesh";
    }

    // Function called by the Player when adding a footprint. 
    // Adds the information needed to create the mesh later. 
    public void AddFootprint(Vector3 pos, Vector3 fwd, Vector3 rht, int footprintType)
    {
        // - Calculate the 4 corners -

        // foot offset
        float footOffset = footprintSpacing;

        if (isLeft)
        {
            footOffset = -footprintSpacing;
        }

        Vector3[] corners = new Vector3[4];

        // corners = position + left/right offset + forward + right
        corners[0] = pos + (rht * footOffset) + (fwd * footprintSize.y * 0.5f) + (-rht * footprintSize.x * 0.5f); // Upper Left
        corners[1] = pos + (rht * footOffset) + (fwd * footprintSize.y * 0.5f) + (rht * footprintSize.x * 0.5f); // Upper Right
        corners[2] = pos + (rht * footOffset) + (-fwd * footprintSize.y * 0.5f) + (-rht * footprintSize.x * 0.5f); // Lower Left
        corners[3] = pos + (rht * footOffset) + (-fwd * footprintSize.y * 0.5f) + (rht * footprintSize.x * 0.5f); // Lower Right


        // raycast to get the position and normal for each corner
        RaycastHit hit = new RaycastHit();

        for (int i = 0; i < 4; i++ )
     {
            Vector3 rayPos = corners[i];
            rayPos.y = 1000.0f;

            if (Physics.Raycast(rayPos, -Vector3.up, out hit, 2000.0f, terrainLayer))
            {
                int index = (footprintCount * 4) + i;

                // - Vertex -
                vertices[index] = hit.point + (hit.normal * groundOffset);

                // - Normal -
                normals[index] = hit.normal;

            }
        }


        // - UVs -

        // what type of footprint is being placed
        Vector2 uvOffset;

        switch (footprintType)
        {
            case 1:
                uvOffset = new Vector2(0.5f, 0.0f);
                break;

            case 2:
                uvOffset = new Vector2(0.0f, 0.0f);
                break;

            case 3:
                uvOffset = new Vector2(0.5f, 0.0f);
                break;

            default:
                uvOffset = new Vector2(0.0f, 0.0f);
                break;
        }

        // is this the left foot or the right foot
        switch (isLeft)
        {
            case true:
                uvs[(footprintCount * 4) + 0] = new Vector2(uvOffset.x + 0.5f, uvOffset.y);
                uvs[(footprintCount * 4) + 1] = new Vector2(uvOffset.x, uvOffset.y);
                uvs[(footprintCount * 4) + 2] = new Vector2(uvOffset.x + 0.5f, uvOffset.y);// - 0.5f);
                uvs[(footprintCount * 4) + 3] = new Vector2(uvOffset.x, uvOffset.y);// - 0.5f);

                isLeft = false;
                break;

            case false:
                uvs[(footprintCount * 4) + 0] = new Vector2(uvOffset.x, uvOffset.y);
                uvs[(footprintCount * 4) + 1] = new Vector2(uvOffset.x + 0.5f, uvOffset.y);
                uvs[(footprintCount * 4) + 2] = new Vector2(uvOffset.x, uvOffset.y);// - 0.5f);
                uvs[(footprintCount * 4) + 3] = new Vector2(uvOffset.x + 0.5f, uvOffset.y);// - 0.5f);

                isLeft = true;
                break;
        }



        // - Triangles -

        triangles[(footprintCount * 6) + 0] = (footprintCount * 4) + 0;
        triangles[(footprintCount * 6) + 1] = (footprintCount * 4) + 1;
        triangles[(footprintCount * 6) + 2] = (footprintCount * 4) + 2;

        triangles[(footprintCount * 6) + 3] = (footprintCount * 4) + 2;
        triangles[(footprintCount * 6) + 4] = (footprintCount * 4) + 1;
        triangles[(footprintCount * 6) + 5] = (footprintCount * 4) + 3;


        // - Increment counter -
        footprintCount++;

        if (footprintCount >= maxFootprints)
        {
            footprintCount = 0;
        }

        // - update mesh with new info -
        ConstructMesh();
    }

    void ConstructMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.uv = uvs;
    }

    public void TurnOnMesh()
    {
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void TurnOffMesh()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
