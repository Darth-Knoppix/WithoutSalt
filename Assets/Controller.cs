using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	private float movementSpeed = 8;
	private float shiftSpeed = 50;
	private int position = 2;
	private int jumpTimer = 0;
	private bool bIsInAir = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.D)) {
			transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.A)) {
			transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
		}

		if (Input.GetKeyUp(KeyCode.S) && position > 1) {
			transform.Translate(Vector3.back * shiftSpeed * 2 * (int) Time.deltaTime);
			position -=1;
		}

		if (Input.GetKeyUp(KeyCode.W) && position < 3) {
			transform.Translate(Vector3.forward * shiftSpeed * 2 * Time.deltaTime);
			position +=1;
		}

		if(Input.GetKey(KeyCode.Space) && !bIsInAir){
			if(jumpTimer == 20){
				jumpTimer = 0;
				transform.Translate(Vector3.up * shiftSpeed * Time.deltaTime);
				bIsInAir = true;
			}
		}
		if (jumpTimer > 20) {
						jumpTimer = 20;
				} else {
						++jumpTimer;
				}

	}
	
}
