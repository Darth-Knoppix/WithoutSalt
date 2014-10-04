using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

public class Patrol : MonoBehaviour {
	public float movementSpeed = 2;
	public bool pausePatrol = false;
	public bool followHero = false;
	private bool switchDir = false;
<<<<<<< HEAD
	public float moveLimit = 15;
	private float origPos;
	// Use this for initialization

	void Start () {
		origPos = transform.position.x;
=======
	private int move = 0;
	public int moveLimit = 15;
	// Use this for initialization

	void Start () {
		
>>>>>>> 93b0d16f50a2377370c6ed2faf39e7a8ebd1abc9
	}

	void OnCollisionEnter (Collision col)
	{
<<<<<<< HEAD

=======
		switchDir = !switchDir;
>>>>>>> 93b0d16f50a2377370c6ed2faf39e7a8ebd1abc9
	}

	// Update is called once per frame
	void Update () {
		
		if (switchDir == false) {
			transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
<<<<<<< HEAD
			//move++;
			if(transform.position.x >= origPos+moveLimit){
=======
			if(transform.position.x >= moveLimit){
>>>>>>> 93b0d16f50a2377370c6ed2faf39e7a8ebd1abc9
				switchDir = true;
			}
			
		} else {
			transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
<<<<<<< HEAD
			//move--;
			if(transform.position.x <= origPos-moveLimit){
=======
			if(transform.position.x <= -moveLimit){
>>>>>>> 93b0d16f50a2377370c6ed2faf39e7a8ebd1abc9
				switchDir = false;
			}
		}
		

	}
	
}