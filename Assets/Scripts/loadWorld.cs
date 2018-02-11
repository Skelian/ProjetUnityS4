using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadWorld : MonoBehaviour {

    public GameObject camera;
    private World world;

	// Use this for initialization
	void Start () {
        BlockDefManager.InitBlockDefinitions();
        Save save = new Save("testSave");
        world = save.GetWorld(0, World.SEED_TEST_WORLD, camera.transform.position);
        world.SaveLoadedChunks();
	}
	
}
