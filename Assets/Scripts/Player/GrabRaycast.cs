using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabRaycast : MonoBehaviour {


    private RaycastHit vision;
    private PhysicsRaycaster pr;
    
    void Start()
    {
        pr = GetComponent<PhysicsRaycaster>();
    }

	void FixedUpdate() {
		if ((Physics.Raycast(GetComponent<Transform>().position, Vector3.forward, out vision, 8.0f)) && (Input.GetKeyDown(ControlePerso.ATTAQUER))
        {
            // Joueur à portée de combat ou bloc à portée de minage : on garde l'objet en mémoire
			if (((vision.collider.tag == "perso") && (vision.distance <= 3)) || ((vision.collider.name.StartsWith("chunk")) && (vision.distance <= 5)))
			{
				vision.collider.gameObject.hit();
			}
        }
    }
}
