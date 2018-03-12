using System.Collections;
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
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer) {


			transform.Rotate (0, Input.GetAxis ("Mouse X") * 5f, 0);


			mv = Input.GetAxis ("Vertical");
			mh = Input.GetAxis ("Horizontal");

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
			
			//Si le joueur saute
			if (Input.GetAxis ("Jump") > 0 && auSol) {
				rb.AddForce (new Vector3 (0, jumpForce, 0), ForceMode.Impulse);
				//Le joueur n'est donc plus sur le sol
				auSol = false;
			}
		}
	}

	void OnCollisionStay(Collision co){
		if (co.contacts [0].normal == new Vector3(0,1,0)) {
			auSol = true;
		}
	}

	void FixedUpdate(){
		if(isLocalPlayer)
			RotateCameraY ();
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
		
	public override void OnStartLocalPlayer ()
	{
		tete = transform.GetChild (0).gameObject;
		tete.transform.GetChild (0).gameObject.SetActive(true);
	}
}
