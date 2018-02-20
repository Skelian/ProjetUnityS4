using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDroppedItemScript : MonoBehaviour {

    public int nbIron;
    public int nbLead;
    public int nbMartianStone;
    public int nbMoonStone;
    public int nbO2;
    public int nbOilOre;
    public int nbSand;
    public int nbTitanium;
    public int nbUranium;

    void Start()
    {
        nbIron = 0;
        nbLead = 0;
        nbMartianStone = 0;
        nbMoonStone = 0;
        nbO2 = 0;
        nbOilOre = 0;
        nbSand = 0;
        nbTitanium = 0;
        nbUranium = 0;
}

    void OnTriggerEnter(Collider objet)
    {

        if (objet.gameObject.CompareTag("DroppedIron"))
        {
            objet.gameObject.SetActive(false);
            nbIron++;
        }
        else if (objet.gameObject.CompareTag("DroppedLead"))
        {
            objet.gameObject.SetActive(false);
            nbLead++;
        }
        else if (objet.gameObject.CompareTag("DroppedMartianStone"))
        {
            objet.gameObject.SetActive(false);
            nbMartianStone++;
        }
        else if (objet.gameObject.CompareTag("DroppedMoonStone"))
        {
            objet.gameObject.SetActive(false);
            nbMoonStone++;
        }
        else if (objet.gameObject.CompareTag("DroppedO2"))
        {
            objet.gameObject.SetActive(false);
            nbO2++;
        }
        else if (objet.gameObject.CompareTag("DroppedOilOre"))
        {
            objet.gameObject.SetActive(false);
            nbOilOre++;
        }
        else if (objet.gameObject.CompareTag("DroppedSand"))
        {
            objet.gameObject.SetActive(false);
            nbSand++;
        }
        else if (objet.gameObject.CompareTag("DroppedTitanium"))
        {
            objet.gameObject.SetActive(false);
            nbTitanium++;
        }
        else if (objet.gameObject.CompareTag("DroppedUranium"))
        {
            objet.gameObject.SetActive(false);
            nbUranium++;
        }
    }
}
