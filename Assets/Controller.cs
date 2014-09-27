using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Controller : MonoBehaviour {
	private Vector3 jumpVelocity = new Vector3(0, 10, 0); //Jump power (Only change Y)
	private Vector3 hop = new Vector3(0, 2, 0); //Jump power when switching planes (Only change Y)
	private Vector3 speed = new Vector3(8, 0, 0); //Movement speed (Only change X)

	private bool isFalling = false;

	private bool PSM = Application.platform == RuntimePlatform.PSM; //Check if platform is PSM
	private bool padAct, padJump, padPause, padThrow, padUp, padRight, padDown, padLeft = false; //Controls
    
	private int plane = 2; //Current plane
	private static int[] planeZ = new int[] {0, 2, 4, 6, 8}; //Only use planeZ[1-3]

	private bool wait = false;
	private int waitCount = 0;
	private int waitTimer = 20;

	// Use this for initialization
	void Start () {

	}

	void OnCollisionStay (Collision col) {
		if (col.collider.tag == "Ground") { //Player is touching the ground
			isFalling = false;
		}
		if (col.collider.tag == "Tree") { //Player is walking into a tree
			print("YOU HIT THE TREE");
		}
	}
	void OnCollisionEnter (Collision col) {

	}
	void OnCollisionExit (Collision col) {
		if (col.collider.tag == "Ground") { //Player is no longer touching the ground
			isFalling = true;
		}
	}

	// Update is called once per frame
	void Update () {
		padAct = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton0); //E Key, Cross Button
		padJump = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton1) || Input.GetKey(KeyCode.JoystickButton4); //Space Key, Circle Button, Left Shoulder Button
		padPause = Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.JoystickButton7); //Escape Key, Start Button
		padThrow = Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton2) || Input.GetKey(KeyCode.JoystickButton5); //Left Mousebutton, Square Button, Right Shoulder Button
		padUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton8); //W Key, Up Key, Up Button
		padRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.JoystickButton9); //D Key, Right Key, Right Button
		padDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.JoystickButton10); //S Key, Down Key, Down Button
		padLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.JoystickButton11); //A Key, Left Key, Left Button

		if (wait) {
			++waitCount;
			if(waitCount >= waitTimer) {
				wait = false;
				waitCount = 0;
			}
		}

		if (padLeft) { //Move left
			rigidbody.MovePosition(rigidbody.position - speed * Time.deltaTime);
		} else if (padRight) { //Move right
			rigidbody.MovePosition(rigidbody.position + speed * Time.deltaTime);
		}

		if (!isFalling && !wait) {
			if (padUp) { //Switch plane (backwards)
				rigidbody.AddForce (hop, ForceMode.VelocityChange);

				if (plane == 1) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, planeZ[2]);;
					plane = 2;
				} else if (plane == 2) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, planeZ[3]);
					plane = 3;
				}
				wait = true;

			}
			if (padDown) { //Switch plane (forwards)
				rigidbody.AddForce (hop, ForceMode.VelocityChange);

				if (plane == 3) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, planeZ[2]);
					plane = 2;
				} else if (plane == 2) {
					transform.position = new Vector3 (transform.position.x, transform.position.y, planeZ[1]);
					plane = 1;
				}
				wait = true;
			}
			if (padJump) { //Jump
				rigidbody.AddForce (jumpVelocity, ForceMode.VelocityChange);
				wait = true;
			}
		}
	}
}