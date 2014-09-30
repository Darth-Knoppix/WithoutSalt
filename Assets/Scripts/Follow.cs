using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public float offset;
	public Transform target;
	private Vector3 newPosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		newPosition.x = target.position.x;
		newPosition.y = target.position.y + offset;
		newPosition.z = ((target.position.z * 2) - 14) / 2;

		transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime * 2);
	}
}
