using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour {

	public float speed, jumpForce;
	private float mh, mv;
	private Vector3 jump, push;
	private Animator ator;
	private Rigidbody rb;
	private float step;
	bool auSol;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		ator = GetComponent<Animator> ();
		jump = new Vector3 (0f, 1.5f, 0f);
		step = speed * Time.deltaTime;
		push = new Vector3 (5f, 0f, 5f);

	}

	// Update is called once per frame
	void Update () {
			dplcmnt ();

	}


	void onTriggerEnter(Collider co){
		if (co.tag == "perso") {
			//co.transform.position
		}
	}
	void OnCollisionEnter(Collision co){
		Debug.Log (co.gameObject.name);
		Debug.Log (co.gameObject.tag);
		if (co.gameObject.tag == "perso") {
			//co.rigidbody.AddForce(push, ForceMode.Impulse);

		}
	}

	Vector3 randVector(){
		Vector3 pos =  new Vector3 (Random.Range (-100f, 100f), 0, Random.Range (-100f, 100f));
		return pos;
	}

	void dplcmnt(){
		if (auSol) {
			rb.AddForce (jump * jumpForce, ForceMode.Impulse);
			auSol = false;
		}
		if (Vector3.Distance (transform.position, GameObject.FindGameObjectWithTag ("perso").transform.position) <= 10) {
			if (auSol) {
				rb.AddForce (jump * jumpForce, ForceMode.Impulse);
				auSol = false;
			} else {
				transform.position = Vector3.MoveTowards (transform.position, GameObject.FindGameObjectWithTag ("perso").transform.position, step);


			}

			/*Vector3 targetDir = GameObject.FindGameObjectWithTag ("perso").transform.position - transform.position;

			Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);

			Debug.DrawRay (transform.position, newDir, Color.red);
			newDir.y = 0f;
			transform.rotation = Quaternion.LookRotation (newDir);*/

			transform.LookAt (GameObject.FindGameObjectWithTag ("perso").transform.position);



		} else {
			//transform.position = Vector3.MoveTowards (transform.position, randVector() , step);
		}
	}

	void OnCollisionStay(Collision co){
		foreach (ContactPoint contact in co.contacts) {
			if (contact.normal == new Vector3 (0, 1, 0)) {
				auSol = true;
			}
		}

	}
}

