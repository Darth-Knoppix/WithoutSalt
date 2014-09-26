using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Controller : MonoBehaviour {
	private float movementSpeed = 8;
	private Vector3 jumpVelocity = new Vector3(0, 10, 0);
	private Vector3 hop = new Vector3(0, 2, 0);
	private Vector3 speed = new Vector3(8, 0, 0);
    
	public int plane = 2;

	private bool isJumping = false;
	private bool isFalling = false;
	private float curJump = 0;
	private float maxJump = 5;
	
	public int planeOne = 2;
	public int planeTwo = 4;
	public int planeThree = 6;

	public int[] planes = {2, 4, 6};

	private bool wait = false;
	private int waitCount = 0;
	private int waitTimer = 20;

	// Use this for initialization
	void Start () {

	}

	void OnCollisionEnter (Collision col)
	{
		Collider boxColl = gameObject.GetComponent<BoxCollider> ();
		if (col.collider.tag == "Ground" && col.collider == boxColl) {
			isFalling = false;
			curJump = 0;
		}
		if (col.collider.tag == "Tree") {
			print("YOU HIT THE TREE");
		}
	}
	void OnCollisionExit (Collision col)
	{

		if (col.collider.tag == "Ground" && !isJumping) {
			isFalling = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if(rigidbody.velocity.y == 0) isFalling = false;

		if (wait) {
			++waitCount;
			if(waitCount >= waitTimer) {
				wait = false;
				waitCount = 0;
			}
		}

		if (Input.GetKey(KeyCode.D)) {
			rigidbody.MovePosition(rigidbody.position + speed * Time.deltaTime);
			//transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.A)) {
			rigidbody.MovePosition(rigidbody.position - speed * Time.deltaTime);
			//transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.W) && !isFalling && !wait) {
			rigidbody.AddForce(hop, ForceMode.VelocityChange);

			if(plane == 1) {
				Vector3 pl2 = new Vector3(transform.position.x, transform.position.y, planeTwo);
				transform.position = pl2;
				plane = 2;
			} else if(plane == 2) {
				Vector3 pl3 = new Vector3(transform.position.x, transform.position.y, planeThree);
				transform.position = pl3;
				plane = 3;
			}

			wait = true;
        }

		if (Input.GetKey (KeyCode.S) && !isFalling && !wait) {
			rigidbody.AddForce(hop, ForceMode.VelocityChange);
			
			if(plane == 3) {
				Vector3 pl2 = new Vector3(transform.position.x, transform.position.y, planeTwo);
				transform.position = pl2;
				plane = 2;
			} else if(plane == 2) {
				Vector3 pl1 = new Vector3(transform.position.x, transform.position.y, planeOne);
				transform.position = pl1;
				plane = 1;
			}
			
			wait = true;
		}

		if (Input.GetKey (KeyCode.Space) && !isFalling && !wait) {
			rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);

			wait = true;
		}


	}
	
}