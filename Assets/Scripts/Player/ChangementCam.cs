using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangementCam : MonoBehaviour {

	public GameObject head;
	public GameObject camTPS;
	bool cameraChange=false;

	void Update () {


		if (Input.GetKeyDown (KeyCode.M)) {
			if (!cameraChange) {
				transform.position = camTPS.transform.position;
				cameraChange = true;
			} else {
				transform.position = head.transform.position;
				cameraChange = false;
			}
		}
	}
}

/* Mettre un objet a l'endroit ou l'on veut mettre la camera 3eme personne.
 * Decocher le box collider.
 * Mettre l'objet dans la variable cameraTPS
 * Mettre la tete du personnage dans la variable head.
 * Mettre le cube en enfant de la tete
 * Mettre le script dans la camera.
 * Mettre le cube en invisible
 */
