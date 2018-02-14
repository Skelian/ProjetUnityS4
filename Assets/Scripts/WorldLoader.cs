using UnityEngine;

public class WorldLoader : MonoBehaviour {

    public static MonoBehaviour instance;

    public GameObject player;
    public int loadDistance;
    private World world;

	// Use this for initialization
	void Start () {
        instance = this;

        BlockDefManager.InitBlockDefinitions();
        BlockDefManager.BakeTextureAtlas();

        Settings.loadDistance = loadDistance;

        Save save = new Save("testSave");
        world = save.GetWorld(0, World.SEED_TEST_WORLD, player.transform.position);

        world.SaveLoadedChunks();
        Debug.Log("all chunks saved");
	}

    void Update()
    {
        world.Update(player.transform.position);
    }
	
}
