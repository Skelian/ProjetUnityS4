using System;
using UnityEngine;

public class WorldLoader : MonoBehaviour
{
    public static WorldLoader instance;

    public GameObject player;
    public int loadDistance;
    private World world;

    // Use this for initialization
    void Start()
    {
        instance = this;
        BlockDefManager.InitBlockDefinitions();
        BlockDefManager.BakeTextureAtlas();

        Chunk.InitChunkObject();
        AsyncChunkOps.Init();

        Settings.loadDistance = loadDistance;

        Save save = new Save("testSave");
        int seed = 179443212;
        world = save.CreateWorld(0, seed, player.transform.position);

        world.GetChunk(new Position(0, 1, 0)).SetLocalBlock(200, new Position(15, 0, 15));  
        world.GetChunk(new Position(0, 1, 0)).SetLocalBlock(200, new Position(5, 5, 5));
    }

    void Update()
    {
        world.Update(player.transform.position);
        //world.GetLoadedChunk(EntityUtils.GetChunkPosition(player.transform.position)).SetLocalBlock(103, EntityUtils.GetBlocToChunkPosition(player.transform.position).Subtract(0, 1, 0));
    }

    public World GetWorld()
    {
        return world;
    }

}
