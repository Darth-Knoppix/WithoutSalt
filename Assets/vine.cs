using UnityEngine;
using System.Collections;

public class vine : MonoBehaviour {
	float liveTime;
	// Use this for initialization
	void Start () {
		liveTime = 120f * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
		liveTime-= Time.deltaTime;
		if (liveTime <= 0f) {
			Destroy(gameObject);
		}
	}
}
