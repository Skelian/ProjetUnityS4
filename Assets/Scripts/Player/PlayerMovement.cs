﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

	public float speed, jumpForce;
	[SerializeField] private GameObject tete;

	private float mh, mv;
	private Vector3 jump;
	private Animator ator;
	private Rigidbody rb;
	private float currenthealth;
	private float tmpSpeed;

	[SerializeField] public bool auSol;

	// Use .this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rb = GetComponent<Rigidbody> ();
		ator = GetComponent<Animator> ();
		//Contrainte de rotation en X, Z activée
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		jump = new Vector3 (0f, 1.5f, 0f);
		tmpSpeed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		if(!GestionMenu.pause && isLocalPlayer){
			RotateCameraX ();

			Deplacement ();

			Saut();
		}
	}

	void FixedUpdate(){
		if((!GestionMenu.pause)&&(!GestionMenu.InvStats)&&isLocalPlayer)
			RotateCameraY ();
	}


	void Deplacement(){
		if (Input.GetKey (ControlePerso.AVANCER))
			mv = 1;
		else if (Input.GetKey (ControlePerso.ARRIERE))
			mv = -1;
		else if(!Input.GetKey (ControlePerso.AVANCER) && !Input.GetKey (ControlePerso.ARRIERE))
			mv = 0;
		
		if (Input.GetKey (ControlePerso.GAUCHE))
			mh = -1;
		else if (Input.GetKey (ControlePerso.DROITE))
			mh = 1;
		else if(!Input.GetKey (ControlePerso.GAUCHE) && !Input.GetKey (ControlePerso.DROITE))
			mh = 0;

		if (Input.GetKey (ControlePerso.COURIR))
			speed = tmpSpeed * 2;
		else
			speed = tmpSpeed;

		if (mv != 0) {
			transform.Translate (mv * transform.forward * speed, Space.World);
			ator.SetBool ("walking", true);
		} else {
			ator.SetBool ("walking", false);
		}
		if (mh != 0) {
			transform.Translate (mh * transform.right * speed, Space.World);
			ator.SetBool ("walking", true);
		}
	}

	void Saut(){
		if (Input.GetKeyDown(ControlePerso.SAUTER) && auSol) {
			rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
			//Le joueur n'est donc plus sur le sol
			auSol = false;
		}
	}

	void RotateCameraX(){
		transform.Rotate (0, Input.GetAxis ("Mouse X") * 5f, 0);
	}
		
	float NegativeEuler(float angle){
		return (angle > 180) ? angle - 360 : angle;
	}

	void RotateCameraY(){
		int up = 65, down = -70;
		// Si t'es l'angle de rotation de la tete est copmprise entre les valeurs desirées
		if (NegativeEuler(tete.transform.rotation.eulerAngles.z) < up && NegativeEuler(tete.transform.rotation.eulerAngles.z) > down) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y") < 0) ? -0.1f : 0.1f) * 25f);
		} else if (NegativeEuler(tete.transform.rotation.eulerAngles.z) > up) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y") < 0) ? -0.1f : 0f) * 25f);

		} else if (NegativeEuler(tete.transform.rotation.eulerAngles.z) < down) {
			if(Input.GetAxis ("Mouse Y") != 0)
				tete.transform.Rotate (0, 0, ((Input.GetAxis ("Mouse Y")> 0) ? 0.1f : 0f) * 25f);
		}
	}

	void OnCollisionStay(Collision co){
		foreach (ContactPoint contact in co.contacts) {
			if (contact.normal == new Vector3 (0, 1, 0)) {
				auSol = true;
			}
		}
	}
		
	public override void OnStartLocalPlayer ()
	{
		tete = transform.GetChild (0).gameObject;
		tete.transform.GetChild (0).gameObject.SetActive(true);
	}
}
