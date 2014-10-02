﻿using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {
	public float offset;
	public Transform target;
	private Vector3 newPosition;
	private float defaultFOV;

	// Use this for initialization
	void Start () {
		defaultFOV = camera.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		newPosition.x = target.position.x;
		newPosition.y = target.position.y + offset;
		newPosition.z = ((target.position.z * 2) - 14) / 2;

		camera.fieldOfView = Mathf.Abs(newPosition.z*2) + defaultFOV;

		transform.position = Vector3.Lerp (transform.position, newPosition, Time.deltaTime * 2);
	}
}
