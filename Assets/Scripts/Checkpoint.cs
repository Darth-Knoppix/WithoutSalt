using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public int saltCost;
	private int health;
	public bool isActivated;
	Controller player;

	// Use this for initialization
	void Start () {
		saltCost = 1;
		player = (Controller) FindObjectOfType (typeof(Controller));
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Player")
		{
			print("OVER");
			if(!isActivated){
				player.setLastCheckpoint(this);
				isActivated = true;
				//play anim
			}
		}
	}

	public void deactivate(){
		isActivated = false;
	}

	public void spawn(){
		player.useSalt (saltCost);
		//Play anim
	}
}
