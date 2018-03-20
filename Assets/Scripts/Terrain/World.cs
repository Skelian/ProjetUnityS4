using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class World
{
    public static int SEED_EMPTY_WORLD = -1;
    public static int SEED_TEST_WORLD = -2;

    /// <summary>
    /// Seed de génération.
    /// </summary>
    public int Seed { get; private set; }

    /// <summary>
    /// Chunk central chargé en mémoire.
    /// </summary>
    public Position CenterChunkPosition { get; private set; }

    /// <summary>
    /// Repertoire ou sont stockés les fichiers de chunks.
    /// </summary>
    public string SaveDir { get; private set; }

    private int loadDistance;
    /// <summary>
    /// Distance de chargement des chunks par rapport au joueur.
    /// </summary>
    public int LoadDistance
    {
        get
        {
            return loadDistance;
        }

        set
        {
            if (value > 0)
            {
                loadDistance = value;
                Reload(CenterChunkPosition, false);
            }
        }
    }

    /// <summary>
    /// Sauvegarde contenant le monde.
    /// </summary>
    public Save ParentSave { get; private set; }

    /// <summary>
    /// Id de la dimension.
    /// </summary>
    public int DimensionID { get; private set; }

    public World(Save save, int dimensionID, int loadDistance, Vector3 playerPosition, int seed)
    {
        Random.InitState(seed);
        Simplex.Noise.Seed = seed;

        this.ParentSave = save;
        this.DimensionID = dimensionID;
        this.Seed = seed;
        this.loadDistance = loadDistance;

        SaveDir = save.GetWorldDir(dimensionID) + "chunks/";
        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);

        //GameObject.Find("Player").GetComponent<Transform>().position = playerPosition;
        Reload(EntityUtils.ToChunkPosition(playerPosition), false);
    }

    public void Save(Vector3 playerPos)
    {
        SaveLoadedChunks();
        using(StreamWriter writer = new StreamWriter(SaveDir + "dim.dat", false))
        {
            writer.Write(Seed);
            writer.Write(playerPos.x);
            writer.Write(playerPos.y);
            writer.Write(playerPos.z);
        }
    }

    /// <summary>
    /// Chunks chargés en mémoire.
    /// </summary>
    public Dictionary<Position, Chunk> LoadedChunks = new Dictionary<Position, Chunk>();
    public Queue<Chunk> ToInstantiate = new Queue<Chunk>();
    public Dictionary<Position, Chunk> DisplayedChunks = new Dictionary<Position, Chunk>();

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    public void Update(Vector3 playerPosition)
    {
        Chunk loaded = AsyncChunkOp.GetLoadedChunk();
        if ((loaded != null) && !LoadedChunks.ContainsKey(loaded.Position))
        {
            LoadedChunks.Add(loaded.Position, loaded);

            if(Position.DistanceBetween(loaded.Position, CenterChunkPosition).AllInfEqTo(LoadDistance))
            {
                DisplayedChunks.Add(loaded.Position, loaded);
                loaded.Instantiate();
            }
        }

        if(ToInstantiate.Count > 0)
        {
            Chunk chunk = ToInstantiate.Dequeue();
            chunk.Instantiate();
        }

        Position playerChunkPosition = EntityUtils.ToChunkPosition(playerPosition);
        if (playerChunkPosition == CenterChunkPosition)
            return;

        //unload chunks
        foreach(Chunk chunk in new List<Chunk>(LoadedChunks.Values))
        {
            if (Position.DistanceBetween(chunk.Position, playerChunkPosition).AnySupTo(loadDistance * 2))
            {
                DisplayedChunks.Remove(chunk.Position);
                LoadedChunks.Remove(chunk.Position);
                chunk.Destroy();
                AsyncChunkOp.AddChunkToSaveList(this, chunk);
            }
        }

        //hide chunks
        foreach(Chunk chunk in new List<Chunk>(DisplayedChunks.Values))
        {
            if (Position.DistanceBetween(chunk.Position, playerChunkPosition).AnySupTo(loadDistance))
            {
                DisplayedChunks.Remove(chunk.Position);
                chunk.Hide();
            }
        }

        Position start = playerChunkPosition.SubtractAll(loadDistance);
        Position end = playerChunkPosition.AddAll(loadDistance);

        //load chunks.
        for (int x = start.X; x <= end.X; x++)
        {
            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int z = start.Z; z <= end.Z; z++)
                {
                    Position tmp = new Position(x, y, z);

                    if (!IsChunkLoaded(tmp))
                    {
                        AsyncChunkOp.AddChunkToLoadList(this, tmp);
                    }
                    else
                    {
                        if (!IsChunkDisplayed(tmp))
                        {
                            Chunk toDisplay = LoadedChunks[tmp];
                            ToInstantiate.Enqueue(toDisplay);
                            DisplayedChunks.Add(tmp, toDisplay);
                        }
                    }
                }
            }
        }

        CenterChunkPosition = playerChunkPosition;
    }

    /// <summary>
    /// Force le rechargement des chunks.
    /// </summary>
    public void Reload(Position centerChunkPosition, bool save = true)
    {
        CenterChunkPosition = centerChunkPosition;

        if(save)
            SaveLoadedChunks();

        AsyncChunkOp.ClearLoadingQueue();
        AsyncChunkOp.ClearLoadedQueue();
        LoadedChunks.Clear();

        foreach (Chunk chunk in DisplayedChunks.Values)
            GameObject.Destroy(chunk.ChunkObject);

        DisplayedChunks.Clear();

        Position start = centerChunkPosition.SubtractAll(loadDistance);
        Position end = centerChunkPosition.AddAll(loadDistance);

        //load chunks
        for (int x = start.X; x <= end.X; x++)
        {
            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int z = start.Z; z <= end.Z; z++)
                {
                    Position tmp = new Position(x, y, z);

                    if(Position.DistanceBetween(tmp, centerChunkPosition).AnySupTo(3))
                    {
                        AsyncChunkOp.AddChunkToLoadList(this, tmp);
                    }
                    else
                    {
                        Chunk chunk = GetChunkFromFile(tmp);
                        LoadedChunks.Add(tmp, chunk);
                        DisplayedChunks.Add(tmp, chunk);
                        chunk.Instantiate();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Envoie True si le chunk à la position spécifiée est chargé en mémoire.
    /// </summary>
    public bool IsChunkLoaded(Position chunkPos)
    {
        return LoadedChunks.ContainsKey(chunkPos);
    }

    public bool IsChunkDisplayed(Position chunkPos)
    {
        return DisplayedChunks.ContainsKey(chunkPos);
    }

    /// <summary>
    /// Retourne le chunk présent aux coordonées indiquées.
    /// </summary>
    public Chunk GetChunk(Position chunkPos)
    {
        return (IsChunkLoaded(chunkPos) ? LoadedChunks[chunkPos] : null);
    }

    private Chunk GetChunkFromFile(Position chunkPos)
    {
        return Chunk.LoadChunk(this, chunkPos);
    }

    /// <summary>
    /// Sauvegarde le terrain.
    /// </summary>
    public void SaveLoadedChunks()
    {
        foreach (Chunk chunk in LoadedChunks.Values)
            AsyncChunkOp.AddChunkToSaveList(this, chunk);
    }

}

