using System.Threading;

public class AsyncChunkOps
{
    private struct LoadingChunk
    {
        public Position position;
        public World world;
    }

    private struct SavingChunk
    {
        public Chunk chunk;
        public World world;
    }

    private static Thread chunkLoader;
    private static Thread chunkSaver;
    private static ConcurrentQueue<SavingChunk> chunksToSave = new ConcurrentQueue<SavingChunk>();
    private static ConcurrentQueue<LoadingChunk> chunksToLoad = new ConcurrentQueue<LoadingChunk>();
    private static ConcurrentQueue<Chunk> loadedChunks = new ConcurrentQueue<Chunk>();

    public static void Init()
    {
        chunkLoader = new Thread(() =>
        {
            while(chunkLoader.IsAlive)
            {
                while (chunksToLoad.Count > 0)
                {
                    LoadingChunk toLoad;
                    chunksToLoad.TryDequeue(out toLoad);

                    Chunk loaded = Chunk.LoadChunk(toLoad.world, toLoad.position);
                    loadedChunks.Enqueue(loaded);
                }

                Thread.Sleep(10);
            }
        });

        chunkSaver = new Thread(() =>
        {
            while (chunkLoader.IsAlive)
            {
                while (chunksToSave.Count > 0)
                {
                    SavingChunk toSave;
                    chunksToSave.TryDequeue(out toSave);
                    Chunk.SaveChunk(toSave.world.SaveDir, toSave.chunk);
                }

                Thread.Sleep(10);
            }
        });

        chunkLoader.Start();
        chunkSaver.Start();
    }

    public static void AddChunkToSaveList(World world, Chunk chunkToSave)
    {
        // ne pas sauvegarder un chunk déja en cours de sauvegarde
        foreach (SavingChunk saving in chunksToSave)
            if (saving.chunk.Position == chunkToSave.Position)
                return;

        SavingChunk toSave;
        toSave.world = world;
        toSave.chunk = chunkToSave;
        chunksToSave.Enqueue(toSave);
    }

    public static void AddChunkToLoadList(World world, Position chunkPos)
    {
        //ne pas charger un chunk déja en cours de chargement
        foreach (LoadingChunk loading in chunksToLoad)
            if (loading.position == chunkPos)
                return;

        LoadingChunk toLoad;
        toLoad.world = world;
        toLoad.position = chunkPos;
        chunksToLoad.Enqueue(toLoad);
    }

    public static Chunk GetLoadedChunk()
    {
        Chunk chunk;
        loadedChunks.TryDequeue(out chunk);
        return chunk;
    }

    public static void ClearLoadingQueue()
    {
        LoadingChunk ignored;
        while (chunksToLoad.TryDequeue(out ignored)) ;
    }

    public static void ClearLoadedQueue()
    {
        Chunk ignored;
        while (loadedChunks.TryDequeue(out ignored)) ;
    }

}

