using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDroppedItemScript : MonoBehaviour {

    public int nbBlankObject = 0;

    void OnTriggerEnter(Collider objet)
    {

        if (objet.gameObject.CompareTag("BlankObject"))
        {
            objet.gameObject.SetActive(false);
            nbBlankObject++;
        }

    }
}
