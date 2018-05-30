using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gregg : MonoBehaviour {

    [Header("Movement Settings")]
    public float walkSpeed;
    public float hurtSpeed;
    float speed;
    float xInput;
    float yInput;
    Vector3 dir;
    Vector3 lastDir;
    bool updatelastDir = false;
    public enum Direction { Up, Down, Left, Right }
    public Direction direction;
    public enum State { Move, Interact, Attack, Stun, Die }
    public State state;

    [Header("Interact Settings")]
    public float checkDist;
    public float checkTime;
    RaycastHit foundHit;

    [Header("Controls References")]
    Rigidbody rb;
    Animator animator;

    [Header("Attack Settings")]
    public GameObject weaponPrefab;
    public float attackTime;

    SpriteRenderer[] characterRenderer;

    // Use this for initialization
    void Start () {
        speed = walkSpeed;

        characterRenderer = GetComponentsInChildren<SpriteRenderer>();
        //foreach (SpriteRenderer mesh in characterRenderer)
        //    mesh.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOnMesh()
    {
        foreach (SpriteRenderer mesh in characterRenderer)
            mesh.enabled = true;
    }

    public void TurnOffMesh()
    {
        foreach (SpriteRenderer mesh in characterRenderer)
            mesh.enabled = false;
    }
}
