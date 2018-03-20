using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlePerso : MonoBehaviour {
	public GameObject btnAttaquer, btnAvancer,btnArriere,btnGauche,btnUtiliser,btnSauter,btnDroite,btnRun;

	public static KeyCode ATTAQUER = KeyCode.Mouse0,
		UTILISER = KeyCode.Mouse1,
		AVANCER = KeyCode.Z,
		ARRIERE = KeyCode.S,
		GAUCHE = KeyCode.Q,
		DROITE = KeyCode.D,
		SAUTER = KeyCode.Space,
		COURIR = KeyCode.LeftShift;

	bool chg = false;
	KeyCode toChg;
	void Update(){
		if (chg) {
			foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
				if (Input.GetKeyDown (kcode)) {
					if (toChg == ATTAQUER) {
						ATTAQUER = kcode;
						btnAttaquer.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == AVANCER) {
						AVANCER= kcode;
						btnAvancer.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == ARRIERE) {
						ARRIERE= kcode;
						btnArriere.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == GAUCHE) {
						GAUCHE = kcode;
						btnGauche.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == DROITE) {
						DROITE = kcode;
						btnDroite.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}

					else if (toChg == UTILISER) {
						UTILISER = kcode;
						btnUtiliser.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == SAUTER) {
						SAUTER = kcode;
						btnSauter.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
					else if (toChg == COURIR) {
						COURIR = kcode;
						btnRun.transform.GetChild(0).GetComponent<Text>().text = kcode.ToString(); 
						chg = false;
					}
				}
			}

		}
	}
		
	public void changeAttaquer(){
		chg = true;
		toChg = ATTAQUER;
	}
	public void changerAvant(){
		chg = true;
		toChg = AVANCER;
	}
	public void changerArriere(){
		chg = true;
		toChg = ARRIERE;
	}
	public void changerGauche(){
		chg = true;
		toChg = GAUCHE;
	}
	public void changerDroite(){
		chg = true;
		toChg = DROITE;
	}
	public void changerUtiliser(){
		chg = true;
		toChg = UTILISER;
	}
	public void changerSauter(){
		chg = true;
		toChg = SAUTER;
	}
	public void changerCourir(){
		chg = true;
		toChg = COURIR;
	}
}
