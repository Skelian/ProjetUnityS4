using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabRaycast : MonoBehaviour {

    public object objectIntersected;

    private RaycastHit vision;
    private PhysicsRaycaster pr;
    
    void Start()
    {
        pr = GetComponent<PhysicsRaycaster>();
    }

    void Update() {

        if (Physics.Raycast(GetComponent<GameObject>().transform.parent.position, Vector3.right, out vision, 5.0f))
        {
            // Joueur à portée de combat ou bloc à portée de minage : on garde l'objet en mémoire
            if (((vision.collider.tag == "Player") && (vision.distance <= 2)) || ((vision.collider.tag == "Block") && (vision.distance <= 3)))
            {
                objectIntersected = vision;
                Debug.Log("object in vision: " + vision.collider.tag);
            } else {
                objectIntersected = null;
            }

        } else {

            objectIntersected = null;

        }
    }
}
