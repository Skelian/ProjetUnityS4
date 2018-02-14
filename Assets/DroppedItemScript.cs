using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemScript : MonoBehaviour {

    private float vitesse = 0.1f;
    public GameObject joueur;

    void FixedUpdate()
    {

        if (Mathf.Abs(this.transform.position.x - joueur.transform.position.x) < 2 &&
            Mathf.Abs(this.transform.position.y - joueur.transform.position.y) < 2 &&
            Mathf.Abs(this.transform.position.z - joueur.transform.position.z) < 2)
        {
            this.transform.Rotate(new Vector3(0, -5, 0));
            transform.position = Vector3.MoveTowards(transform.position, joueur.transform.position, vitesse * Time.deltaTime);
            if (vitesse < 1.5f) vitesse += 0.05f;
        }
        else
        {
            this.transform.Rotate(new Vector3(0, 5, 0));
            vitesse = 0.1f;
        }
    }

}
