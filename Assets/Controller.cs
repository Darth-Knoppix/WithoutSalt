using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Controller : MonoBehaviour {
	private bool isFalling;
	private bool PSM; 																				//Check if platform is PSM
	private bool padAct, padJump, padPause, padThrow, padUp, padRight, padDown, padLeft; 			//Controls
	private bool wait;

	private int waitCount;
	private int waitTimer;
	private int plane; 																				//Current plane
	private static int[] planeZ; 																	//Only use planeZ[1-3]

	private float movementX;

	private Vector3 jumpVelocity; 																	//Jump power (Only change Y)
	private Vector3 hop; 																			//Jump power when switching planes (Only change Y)
	private Vector3 speed; 																			//Movement speed (Only change X)

	// Use this for initialization
	void Start () {
		isFalling = false;
		PSM = Application.platform == RuntimePlatform.PSM;
		padLeft = false;
		wait = false;
		
		waitCount = 0;
		waitTimer = 20;
		plane = 2;
		planeZ = new int[] {0, 2, 4, 6, 8};
		
		movementX = 0.0f;

		jumpVelocity = new Vector3 (0, 10, 0);
		hop = new Vector3(0, 2, 0);
		speed = new Vector3 (8, 0, 0);
	}

	void OnGUI() {
		//Debugging Vertical and horizontal movement
		GUI.Label(new Rect(10, 10, 100, 20), Input.GetAxis("Vertical").ToString());
		GUI.Label(new Rect(10, 30, 100, 20), movementX.ToString());
	}
	
	void OnCollisionStay (Collision col) {
		if (col.collider.tag == "Ground") { 														//Player is touching the ground
			isFalling = false;
		}
		if (col.collider.tag == "Tree") { 															//Player is walking into a tree
			print("YOU HIT THE TREE");
		}
	}
	void OnCollisionEnter (Collision col) {

	}
	void OnCollisionExit (Collision col) {
		if (col.collider.tag == "Ground") { 														//Player is no longer touching the ground
			isFalling = true;
		}
	}

	// Update is called once per frame
	void Update () {
		getInput ();
		if (wait) {
			++waitCount;
			if(waitCount >= waitTimer) {
				wait = false;
				waitCount = 0;
			}
		}
		rigidbody.MovePosition (rigidbody.position + (speed * movementX * Time.smoothDeltaTime));

		if (!isFalling && !wait) {
			int currentPosition = plane;
			if (padUp) { //Switch plane (backwards)
				rigidbody.AddForce (hop, ForceMode.VelocityChange);

				if (plane == 1) {
					plane = 2;
				} else if (plane == 2) {
					plane = 3;
				}
				wait = true;
			}
			if (padDown) { //Switch plane (forwards)
				rigidbody.AddForce (hop, ForceMode.VelocityChange);

				if (plane == 3) {
					plane = 2;
				} else if (plane == 2) {
					plane = 1;
				}
				wait = true;
			}

			transform.position = new Vector3 (transform.position.x, transform.position.y, Mathf.Lerp(planeZ[currentPosition], planeZ[plane], 2));

			if (padJump) { //Jump
				rigidbody.AddForce (jumpVelocity, ForceMode.VelocityChange);
				wait = true;
			}
		}
		
	}

	void FixedUpdate(){
				
		}

	void getInput(){
		padAct 		= Input.GetKey(KeyCode.E) 		|| Input.GetKey(KeyCode.JoystickButton0); 	//E Key, Cross Button
		padJump 	= Input.GetKey(KeyCode.Space) 	|| Input.GetKey(KeyCode.JoystickButton1) 
													|| Input.GetKey(KeyCode.JoystickButton4); 	//Space Key, Circle Button, Left Shoulder Button
		padPause 	= Input.GetKey(KeyCode.Escape) 	|| Input.GetKey(KeyCode.JoystickButton7); 	//Escape Key, Start Button
		padThrow 	= Input.GetMouseButton(0) 		|| Input.GetKey(KeyCode.JoystickButton2) 
													|| Input.GetKey(KeyCode.JoystickButton5); 	//Left Mousebutton, Square Button, Right Shoulder Button


		float tempVert = Input.GetAxis("Vertical");												//Vertical movement for left analogue stick, arrow keys and w-s
		padDown =  	tempVert <= -0.8 				|| Input.GetKey (KeyCode.JoystickButton10)
													|| Input.GetKey(KeyCode.DownArrow);
		padUp 	= 	tempVert >=  0.8				|| Input.GetKey (KeyCode.JoystickButton8)	
													|| Input.GetKey(KeyCode.UpArrow);


		movementX 	= Input.GetAxis("Horizontal");												//Horizontal movement for left analogue stick, arrow keys and a-d
		if (Input.GetKey (KeyCode.JoystickButton11))	movementX = -1;
		if (Input.GetKey (KeyCode.JoystickButton9))		movementX = 1;
	}
}