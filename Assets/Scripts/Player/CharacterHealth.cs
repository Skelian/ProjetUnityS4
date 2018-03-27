using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour {

	public float CurrentHealth { get; set; }
	public float MaxHealth { get; set; }

	public Slider healthbar;

	// Use this for initialization
	void Start () {
		MaxHealth = 20f;
		CurrentHealth = MaxHealth;

		healthbar.value = CalculateHealth();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.X))
			DealDamage (6);
	}

	void DealDamage(int damageValue)
	{

		CurrentHealth -= damageValue;
		healthbar.value = CalculateHealth();

		if(CurrentHealth <= 0)
			Die();
	}

	float CalculateHealth()
	{
		return (CurrentHealth / MaxHealth);
	}

	void Die()
	{
		CurrentHealth = 0;
		Debug.Log("Mort");
	}
}

