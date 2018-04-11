using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class Menu : MonoBehaviour {
	public GameObject mainM;
	public GameObject createW;
	public GameObject loadW;
	public GameObject multi;
	public GameObject options;
	public GameObject GameCtrl;
	public GameObject rejServ;
	public GameObject créerServ;
	public InputField seedTxt;
	public InputField seedTxt2;
	public InputField port;

	public void GoToMainMenu(){
		createW.SetActive (false);
		loadW.SetActive (false);
		options.SetActive (false);
		multi.SetActive (false);
		mainM.SetActive (true);
	}
	public void MultiplayerMenu(){
		multi.SetActive (true);
		mainM.SetActive (false);
	}
	public void rejoindreServeur(){
		multi.SetActive (false);
		rejServ.SetActive (true);
	}
	public void créerServeur(){
		multi.SetActive (false);
		créerServ.SetActive (true);
		int seed = generateSeed ();
		port.text = "7777";
	}
	public void annulerMulti(){
		rejServ.SetActive (false);
		créerServ.SetActive (false);
		multi.SetActive (true);
	}
	public void CreateWorld(){
		createW.SetActive (true);
		mainM.SetActive (false);
		int seed = generateSeed ();

	}	
	public void LoadWorld(){
		loadW.SetActive (true);
		mainM.SetActive (false);
	}
	public void Options(){
		options.SetActive (true);
		mainM.SetActive (false);
	}
	public void GameControl(){
		GameCtrl.SetActive (true);
		options.SetActive (false);
	}
	public void Enregistré(){
		GameCtrl.SetActive (false);
		options.SetActive (true);
	}
	public void Quitter(){
		Application.Quit ();
	}

	public void CreateNewWorld(){
		
	}

	private int generateSeed(){
		String seedT = "AZERTYUIOPQSDFGHJKLMWXCVBNazertyuiopqsdfghjklmwxcvbn0147852369";
		String seed = "";
		System.Random rand = new System.Random ();
		for (int i = 0; i < 30; i++) {
			seed += seedT [rand.Next (seedT.Length)];
		}

		print ("SEED: " + seed + " HASHCODE : " + seed.GetHashCode ().ToString ());
		seedTxt.text = seed;
		seedTxt2.text = seed;

		return seed.GetHashCode ();
	}
}
