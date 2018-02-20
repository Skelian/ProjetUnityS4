using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saut : MonoBehaviour {

	private BoxCollider parterre;

	// Use this for initialization
	void Start () {
		parterre = GetComponent<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionStay(){
		
	}
}


