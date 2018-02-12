using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadWorld : MonoBehaviour {

    public GameObject player;
    private World world;

	// Use this for initialization
	void Start () {
        BlockDefManager.InitBlockDefinitions();
        Settings.loadDistance = 1;
        Save save = new Save("testSave");
        world = save.GetWorld(0, World.SEED_TEST_WORLD, player.transform.position);

        player.transform.position = new Vector3(5, 20, 5);
        world.SaveLoadedChunks();
        Debug.Log("all chunks saved");
	}
	
}
