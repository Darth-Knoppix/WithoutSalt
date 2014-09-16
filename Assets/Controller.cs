using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Controller : MonoBehaviour {
	private float movementSpeed = 8;
	private float shiftSpeed = 50;
	private float jumpSpeed = 40;
	private int position = 0;
	private bool jumped = false;

	private float x = 0;
	private float z = 0;
	private bool hit = false;
	// Use this for initialization
	void Start () {

	}

	void OnCollisionEnter (Collision col)
	{

		if (col.collider.tag == "Ground") {
				jumped = false;
		}
		if (col.collider.tag == "Tree") {
			print("YOU HIT THE TREE");
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.A)) {
			transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKeyUp(KeyCode.S) && position >= 0) {
			transform.Translate(Vector3.back * shiftSpeed * 2 * Time.deltaTime);
			position--;
			x = transform.position.x;
			z = transform.position.z;
		}

		else if (Input.GetKeyUp(KeyCode.W) && position <= 0) {
			transform.Translate(Vector3.forward * shiftSpeed * 2 * Time.deltaTime);
			position++;
			x = transform.position.x;
			z = transform.position.z;
		}
		//if(Input.GetKey(KeyCode.Space) && !bIsInAir){
		if(Input.GetKey(KeyCode.Space) && jumped == false){
				transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
				jumped = true;
				Console.WriteLine(jumped.ToString());
		}



		//if (jumpTimer > 20) {
		//				jumpTimer = 20;
		//		} else {
		//				++jumpTimer;
		//		}

	}
	
}