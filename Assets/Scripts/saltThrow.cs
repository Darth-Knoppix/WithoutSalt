using UnityEngine;
using System.Collections;

public class saltThrow : MonoBehaviour {
	public Object toCreate;

	Vector3 origin;
	Vector3 target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void throwSalt(Vector3 origin, Vector3 target){
		//if();
		this.origin = origin;
		this.target = target;
	}

	void FixedUpdate(){
		//rigidbody.AddForce (Vector3.Normalize (origin + target));
		transform.position = target;
		//rigidbody.rotation = Quaternion.FromToRotation (origin, target);
	}

	void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		Instantiate(toCreate, pos, rot);
		Destroy(gameObject);
	}
}
