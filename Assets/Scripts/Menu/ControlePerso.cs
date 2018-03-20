using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


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


	void Start(){
		Load ();
	}

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

	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/controles.gd");
		List<KeyCode> keycodes = new List<KeyCode>();

		keycodes.Add (ATTAQUER);
		keycodes.Add (UTILISER);
		keycodes.Add (AVANCER);
		keycodes.Add (GAUCHE);
		keycodes.Add (DROITE);
		keycodes.Add (ARRIERE);
		keycodes.Add (COURIR);
		keycodes.Add (SAUTER);

		bf.Serialize(file, keycodes);
		file.Close();
	}

	public void DefaultControls(){
		ATTAQUER = KeyCode.Mouse0;
		UTILISER = KeyCode.Mouse1;
		AVANCER = KeyCode.Z;
		ARRIERE = KeyCode.S;
		GAUCHE = KeyCode.Q;
		DROITE = KeyCode.D;
		SAUTER = KeyCode.Space;
		COURIR = KeyCode.LeftShift;
		UpdateKeycodes ();
	}

	public void Load(){
		if(File.Exists(Application.persistentDataPath + "/controles.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/controles.gd", FileMode.Open);

			List<KeyCode> keycodes = (List<KeyCode>)bf.Deserialize(file);
			ATTAQUER = keycodes [0];
			UTILISER = keycodes [1];
			AVANCER = keycodes [2];
			GAUCHE = keycodes [3];
			DROITE = keycodes [4];
			ARRIERE = keycodes [5];
			COURIR = keycodes [6];
			SAUTER = keycodes [7];

			UpdateKeycodes ();

			file.Close();
		}
	}

	public void UpdateKeycodes(){
		btnAttaquer.transform.GetChild(0).GetComponent<Text>().text = ATTAQUER.ToString(); 
		btnUtiliser.transform.GetChild(0).GetComponent<Text>().text = UTILISER.ToString(); 
		btnGauche.transform.GetChild(0).GetComponent<Text>().text = GAUCHE.ToString(); 
		btnDroite.transform.GetChild(0).GetComponent<Text>().text = DROITE.ToString(); 
		btnAvancer.transform.GetChild(0).GetComponent<Text>().text = AVANCER.ToString(); 
		btnArriere.transform.GetChild(0).GetComponent<Text>().text = ARRIERE.ToString(); 
		btnRun.transform.GetChild(0).GetComponent<Text>().text = COURIR.ToString(); 
		btnSauter.transform.GetChild(0).GetComponent<Text>().text = SAUTER.ToString(); 

	}
}
