using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    public GameObject player;
    public int loadDistance;
    private World world;

    // Use this for initialization
    void Start()
    {
        BlockDefManager.InitBlockDefinitions();
        BlockDefManager.BakeTextureAtlas();

        Chunk.InitChunkObject();

        Settings.loadDistance = loadDistance;

        Save save = new Save("testSave");
        int seed = 66656599;
        world = save.GetWorld(0, World.SEED_TEST_WORLD, player.transform.position);

        world.GetLoadedChunk(new Position(0, 0, 0)).SetLocalBlockBatch(103, new Position(0, 0, 0), new Position(15, 15, 15));
    }

    void Update()
    {
        world.Update(player.transform.position);
        //world.GetLoadedChunk(EntityUtils.GetChunkPosition(player.transform.position)).SetLocalBlock(103, EntityUtils.GetBlocToChunkPosition(player.transform.position).Subtract(0, 1, 0));
    }

}
