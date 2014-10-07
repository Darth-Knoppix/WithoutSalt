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
	public bool switchDir = false;
	public float moveLimit = 15;
	private float origPos;
	// Use this for initialization

	void Start () {
		origPos = transform.position.x;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.collider.tag.Equals ("Enemy")) {
			switchDir = !switchDir;
		}
	}

	// Update is called once per frame
	void Update () {
		
		if (switchDir == false) {
			transform.Translate (Vector3.right * movementSpeed * Time.deltaTime);
			//move++;
			if(transform.position.x >= origPos+moveLimit){
				switchDir = true;
			}
			
		} else {
			transform.Translate (Vector3.left * movementSpeed * Time.deltaTime);
			//move--;
			if(transform.position.x <= origPos-moveLimit){
				switchDir = false;
			}
		}
		

	}
	
}