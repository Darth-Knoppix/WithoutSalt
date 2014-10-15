using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Controller : MonoBehaviour {															
	private bool padAct, padJump, padPause, padThrow, padUp, padRight, padDown; 					//Controls
	private bool wait;
	private bool backClear, forwardClear;
	private bool changeZ;

	private int waitCount, waitTimer, killY;
	private int saltNum, health, saltUsed, deaths;
	private int plane, targetPlane; 																//Current plane
	private static int[] planeZ = {0, 2, 4, 6, 8};													//Only use planeZ[1-3]

	private Vector3 moveDirection = Vector3.zero;
	private Vector3 moveVel = Vector3.zero;
	private static Vector3 groundOffset = new Vector3(0,-1,0);
	private static Vector3 forwardZ = new Vector3(0,0,-5f);
	private static Vector3 backZ = new Vector3(0,0,5f);

	public Texture saltTexture;

	Checkpoint lastCheckpoint;
	CharacterController controller;
	public GameObject projectile;

	public float speed;
	public float jumpSpeed;
	public float hopSpeed;
	public float gravity;
	public int maxHealth = 100;

	public int _uvTieX = 24;
	public int _uvTieY = 1;
	public int _fps = 24;
	
	private Vector2 _size;
	private Renderer _myRenderer;
	private int _lastIndex = -1;

	private float movementX;
	private float movementZ;
	private float iZ;
	private float currentLerpTime, lerpTime;
	
	// Use this for initialization
	void Start () {
		//PSM = Application.platform == RuntimePlatform.PSM;
		wait = false;
		changeZ = false;

		plane = planeZ[2];
		killY = -5;
		targetPlane = plane;
		waitCount = 0;
		waitTimer = 20;
		lerpTime = 0.4f;

		speed = 6.0f;
		jumpSpeed = 10.0f;
		hopSpeed = 4.0f;
		gravity = 20.0f;
		movementX = 0f;
		movementZ = 0f;

		saltNum = 5;
		health = maxHealth;

		controller = GetComponent<CharacterController>();

		/*
		_myRenderer = renderer;
		if(_myRenderer == null)
			enabled = false;
			*/
	}

	void OnGUI() {
		//Debugging Vertical and horizontal movement
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.DrawTexture(new Rect(10,10,32,32), saltTexture, ScaleMode.StretchToFill, true, 10.0F);
		GUI.Label(new Rect(22, 38, 100, 20), saltNum.ToString());
		//GUI.Label(new Rect(10, 10, 100, 20), Input.GetAxis("axisY").ToString());
		//GUI.Label(new Rect(10, 30, 100, 20), movementX.ToString());

		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		GUI.Label(new Rect(Screen.width - 110, 10, 100, 20), "X" + movementX);
		GUI.Label(new Rect(Screen.width - 110, 30, 100, 20), Input.GetAxis("axis5").ToString());
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < killY) respawn ();

		if (wait) {
			++waitCount;
			if(waitCount >= waitTimer) {
				wait = false;
				waitCount = 0;
			}
		}
		/*

		if(movementX == -1){
			_size = new Vector2 (-(1.0f / 27.4f) , 1.0f / _uvTieY);
		}else if(movementX == 1){
			_size = new Vector2 (1.0f / 27.4f , 1.0f / _uvTieY);
		}
		if(movementX != 0){
			// Calculate index
			int index = (int)(Time.timeSinceLevelLoad * _fps) % (_uvTieX * _uvTieY);
			if(index != _lastIndex)
			{
				// split into horizontal and vertical index
				int uIndex = index % _uvTieX;
				int vIndex = index / _uvTieY;
				
				// build offset
				// v coordinate is the bottom of the image in opengl so we need to invert.
				Vector2 offset = new Vector2 (uIndex * _size.x, 1.0f - _size.y - vIndex * _size.y);
				
				_myRenderer.material.SetTextureOffset ("_MainTex", offset);
				_myRenderer.material.SetTextureScale ("_MainTex", _size);
				
				_lastIndex = index;
			}
		}
		*/

		//Change for all controls
		/*
		if (Input.GetMouseButtonDown(0) && saltNum > 0) {
			Vector3 tar = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, plane));
			Instantiate(projectile, new Vector3(transform.position.x, transform.position.y+2, transform.position.z) ,Quaternion.FromToRotation (transform.position, tar));
			//Instantiate(projectile, transform.position,Quaternion.identity);
			projectile.GetComponent<saltThrow>().throwSalt(transform.position, tar);
			saltNum--;
		}
		*/

		getInput ();
		/*
		if (controller.collisionFlags == CollisionFlags.None)
			print("In air");
		
		if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
			print("Touching side");
		
		if (controller.collisionFlags == CollisionFlags.Sides)
			print("Only touching sides");
		
		if ((controller.collisionFlags & CollisionFlags.Above) != 0)
			print("Touching ceiling");
		
		if (controller.collisionFlags == CollisionFlags.Above)
			print("Only touching ceiling");
		
		if ((controller.collisionFlags & CollisionFlags.Below) != 0)
			print("Touching ground");
		
		if (controller.collisionFlags == CollisionFlags.Below)
			print("Only touching ground");
			*/

		backClear = 	!(Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.forward), 1f) 	
		               || Physics.Raycast(new Ray(transform.position+Vector3.right, Vector3.forward), 1f)
		               || Physics.Raycast(new Ray(transform.position+Vector3.left+Vector3.up, Vector3.forward), 1f) 	
		               || Physics.Raycast(new Ray(transform.position+Vector3.right+Vector3.up, Vector3.forward), 1f)
		               || Physics.Raycast(new Ray(transform.position+Vector3.left+Vector3.down, Vector3.forward), 1f) 	
		               || Physics.Raycast(new Ray(transform.position+Vector3.right+Vector3.down, Vector3.forward), 1f)
		               || (Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.forward+Vector3.left),  1.5f) && movementX == -1)
		               || (Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.forward+Vector3.right), 1.5f) && movementX ==  1));

		forwardClear = 	!(Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.back), 1f) 		
		               || Physics.Raycast(new Ray(transform.position+Vector3.right, Vector3.back), 1f)
		               || Physics.Raycast(new Ray(transform.position+Vector3.left+Vector3.up, Vector3.back), 1f) 		
		               || Physics.Raycast(new Ray(transform.position+Vector3.right+Vector3.up, Vector3.back), 1f)
		               || Physics.Raycast(new Ray(transform.position+Vector3.left+Vector3.down, Vector3.back), 1f) 		
		               || Physics.Raycast(new Ray(transform.position+Vector3.right+Vector3.down, Vector3.back), 1f)
		               || (Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.back+Vector3.left), 	1.5f) && movementX == -1)
		               || (Physics.Raycast(new Ray(transform.position+Vector3.left, Vector3.back+Vector3.right), 1.5f) && movementX == 1));

		if(!wait && !changeZ && movementZ < 0 && plane > 2 && iZ == targetPlane && forwardClear){

			targetPlane -= 2;
			wait = true;
		}

		if(!wait && !changeZ && movementZ > 0 && plane < 6 && iZ == targetPlane && backClear){
			targetPlane+= 2;
			wait = true;
		}

		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}

		if (iZ == targetPlane) {
			plane = targetPlane;
			currentLerpTime = 0f;
		}

		iZ = Mathf.Lerp (plane, targetPlane, currentLerpTime / lerpTime);

		if (changeZ && iZ % 1 == 0) {
			wait = true;
			waitCount = 15;
		} changeZ = iZ % 1 != 0;

		if (controller.isGrounded) {
			moveDirection = new Vector3 (movementX, 0, 0);
			moveDirection = transform.TransformDirection (moveDirection);

			moveDirection *= speed;
			moveVel = moveDirection;
			if (!changeZ && padJump) {
				moveDirection.y = jumpSpeed;
			} else if (changeZ) {
				moveDirection.y = hopSpeed;
			}
		} else {
			moveDirection.x = movementX * 0.8f * speed;
			moveDirection = transform.TransformDirection (moveDirection);
		}
		transform.position = new Vector3(transform.position.x, transform.position.y, iZ);
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
		

	public void addSalt(int num){
		saltNum+= num;
	}

	public bool useSalt(int num){
		if (saltNum - num >= 0) {
			saltNum -= num;
			return true;
		}
		return false;
	}

	public void takeDamage(int amount){
		health -= amount;
		if (health <= 0) {
			respawn();
		}
	}

	private void respawn(){
		deaths++;
		if (lastCheckpoint == null) {
			print ("DIED NO CHECKPOINT");
						return;
				}
		if (saltNum >= lastCheckpoint.saltCost) {
			print ("RESPAWN");
			lastCheckpoint.spawn();
			transform.position = new Vector3 (lastCheckpoint.transform.position.x, lastCheckpoint.transform.position.y, planeZ[2]);
			health = maxHealth;
		}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic || !padAct)
			return;
		
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * speed;
	}

	public void setLastCheckpoint(Checkpoint point){
		if (lastCheckpoint != null) {
			lastCheckpoint.deactivate ();
		}
		print ("CHECKPOINT REACHED");
		lastCheckpoint = point;
	}

	void OnTriggerStay(Collider other) {
		if (other.name.Equals ("SaltPickup") && padAct) {
			SaltPickup p = other.gameObject.GetComponent<SaltPickup>();
			saltNum+= p.value;
			p.pickupAnimation();
			Destroy (other.gameObject);
		}
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
		
		if (Input.GetKey (KeyCode.DownArrow)							//Down Key
						|| Input.GetKey (KeyCode.S)					//S Key
						|| Input.GetAxis ("axis7") == 1				//D Pad Down
						|| tempVert <= -0.6) {						//Left Analog Stick Y Down
						movementZ = -2;
				} else if (Input.GetKey (KeyCode.W)				//W Key
						|| Input.GetKey (KeyCode.UpArrow)		//Up Key
						|| Input.GetAxis ("axis7") == -1		//D Pad Up
						|| tempVert >= 0.6) {					//Left Analog Stick Y Up
						movementZ = 2;
				} else {
			movementZ = 0;
				}

		if(			   Input.GetKey(KeyCode.D)					//D Key
		   			|| Input.GetKey(KeyCode.RightArrow)			//Right Key
		   			|| Input.GetAxis("axis6") == -1				//D Pad Right
		) movementX = 1;
		else if(	   Input.GetKey(KeyCode.A)					//A Key
		   			|| Input.GetKey(KeyCode.LeftArrow)			//Left Arrow Key
		   			|| Input.GetAxis("axis6") == 1				//D Pad Left
		) movementX = -1;

	}

	void OnCollisionEnter (Collision col)
	{
		if(col.collider.tag.Equals("Enemy")){
			respawn ();
		}
	}
}