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
	private int move = 0;
	public int moveLimit = 15;
	// Use this for initialization

	void Start () {
		
	}

	void OnCollisionEnter (Collision col)
	{
		switchDir = !switchDir;
	}

	// Update is called once per frame
	void Update () {
		
		if (switchDir == false) {
			transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
			if(transform.position.x >= moveLimit){
				switchDir = true;
			}
			
		} else {
			transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
			if(transform.position.x <= -moveLimit){
				switchDir = false;
			}
		}
		

	}
	
}