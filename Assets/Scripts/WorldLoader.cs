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

        Chunk.InitChunkObject();

        Settings.loadDistance = loadDistance;

        Save save = new Save("testSave");
        world = save.GetWorld(0, World.SEED_TEST_WORLD, player.transform.position);

        //world.GetChunk(new Position(0, 0, 0)).SetLocalBlockBatch(Block.AIR_BLOCK_ID, new Position(0, 14, 0), new Position(15, 15, 15));
	}

    void Update()
    {
        world.Update(player.transform.position);
    }
	
}
