using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed, jumpForce;


	private float mh, mv;
	private Vector3 jump;
	private Animator ator;

	private Rigidbody rb;

	[SerializeField] bool auSol;

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rb = GetComponent<Rigidbody> ();
		ator = GetComponent<Animator> ();
		//Contrainte de rotation en X, Z activée
		rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		jump = new Vector3 (0f, 1.5f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, Input.GetAxis ("Mouse X") * 5f, 0);

		mv = Input.GetAxis ("Vertical");
		mh = Input.GetAxis ("Horizontal");

		if (mv != 0) {
			transform.Translate (mv * transform.forward * speed, Space.World);
			ator.SetBool ("walking", true);
		} else {
			ator.SetBool ("walking", false);
		}
		if (mh != 0){
			transform.Translate (mh * transform.right * speed, Space.World);
		}
			
		//Si le joueur saute
		if (Input.GetAxis ("Jump") > 0 && auSol) {
			rb.AddForce (jump * jumpForce, ForceMode.Impulse);
			//Le joueur n'est donc plus sur le sol
			auSol = false;
		}

			
	}

	void OnCollisionEnter(Collision co){
		if (co.gameObject.tag == "Block") {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
	}

	//Le joueur est au sol
	void OnCollisionStay(){
		auSol = true;
	}
}
