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
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(new Rect(10, 10, 100, 20), Input.GetAxis("axisY").ToString());
		GUI.Label(new Rect(10, 30, 100, 20), movementX.ToString());

		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect(Screen.width - 110, 10, 100, 20), Input.GetAxis("axis4").ToString());
		GUI.Label(new Rect(Screen.width - 110, 30, 100, 20), Input.GetAxis("axis5").ToString());
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
		float tempVert = Input.GetAxis("axisY");				//Left Analog Y
		movementX = Input.GetAxis("axisX"); 					//Left Analog X

		padAct 		=  Input.GetKey(KeyCode.E)					//E Key
					|| Input.GetKey(KeyCode.JoystickButton0);	//Cross Button / A Button

		padJump 	=  Input.GetKey(KeyCode.Space)				//Space Key
					|| Input.GetKey(KeyCode.JoystickButton1)	//Circle Button / B Button
					|| Input.GetKey(KeyCode.JoystickButton4); 	//Left Shoulder Button

		padPause 	=  Input.GetKey(KeyCode.Escape)				//Escape Key
					|| Input.GetKey(KeyCode.JoystickButton7); 	//Start Button

		padThrow 	=  Input.GetMouseButton(0)					//Left Click
					|| Input.GetKey(KeyCode.JoystickButton2)	//Square Button / X Button
					|| Input.GetKey(KeyCode.JoystickButton5); 	//Right Shoulder Button
		
		padDown 	=  Input.GetKey(KeyCode.DownArrow)			//Down Key
					|| Input.GetKey(KeyCode.S)					//S Key
					|| Input.GetAxis("axis7") == 1				//D Pad Down
					|| tempVert <= -0.6;						//Left Analog Stick Y Down

		padUp		=  Input.GetKey(KeyCode.W)					//W Key
					|| Input.GetKey(KeyCode.UpArrow)			//Up Key
					|| Input.GetAxis("axis7") == -1				//D Pad Up
					|| tempVert >= 0.6;							//Left Analog Stick Y Up

		if(			   Input.GetKey(KeyCode.D)					//D Key
		   			|| Input.GetKey(KeyCode.RightArrow)			//Right Key
		   			|| Input.GetAxis("axis6") == -1				//D Pad Right
		) movementX = 1;
		else if(	   Input.GetKey(KeyCode.A)					//A Key
		   			|| Input.GetKey(KeyCode.LeftArrow)			//Left Arrow Key
		   			|| Input.GetAxis("axis6") == 1				//D Pad Left
		) movementX = -1;

	}
}