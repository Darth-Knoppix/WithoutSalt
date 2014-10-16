using UnityEngine;
using System.Collections;

public class restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		animation.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!animation.isPlaying) {
			Application.LoadLevel("EditedNoExtra");
		}
	}
}
