using UnityEngine;
using System.Collections;

public class endGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void onCollisionEnter(Collision col){
		print ("GG");
		Application.LoadLevel ("End");
	}

	void onTriggerEnter(Collision col){
		print ("GG");
		Application.LoadLevel ("End");
	}
	void onCollisionStay(Collision col){
		print ("GG");
		Application.LoadLevel ("End");
	}
	
	void onTriggerStay(Collision col){
		print ("GG");
		Application.LoadLevel ("End");
	}
	// Update is called once per frame
	void Update () {
	
	}
}
