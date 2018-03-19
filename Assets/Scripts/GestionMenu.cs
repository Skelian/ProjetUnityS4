using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionMenu : MonoBehaviour {

    public GameObject Inventory;
    public GameObject InvotoryRapide;
    public GameObject Pause;
    public static bool InvStats { get; private set; }
    

    public static bool pause { get; private set; }

    // Use this for initialization
    void Start () {
        InvStats = false;
        pause = false;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!pause)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {

                InvStats = !InvStats;
                Inventory.SetActive(InvStats);
                Cursor.visible = !Cursor.visible;
                InvotoryRapide.SetActive(!InvStats);

            }

        }
        

        if (Input.GetKeyDown(KeyCode.P))
        {
            pause = !pause;
            Pause.SetActive(pause);

        }
    }
}
