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

        Reload(EntityUtils.ToChunkPosition(playerPosition), false);
    }


    /// <summary>
    /// Chunks chargés en mémoire.
    /// </summary>
    public Dictionary<Position, Chunk> LoadedChunks = new Dictionary<Position, Chunk>();
    public Queue<Position> chunksToLoad = new Queue<Position>();

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    public void Update(Vector3 playerPosition)
    {
        if(chunksToLoad.Count > 0)
        {
            Position pos = chunksToLoad.Dequeue();
            LoadedChunks.Add(pos, GetChunkFromFile(pos));
        }

        Position playerChunkPosition = EntityUtils.ToChunkPosition(playerPosition);
        if (playerChunkPosition == CenterChunkPosition)
            return;

        //delete chunks.
        foreach(Chunk chunk in new List<Chunk>(LoadedChunks.Values))
        {
            if (Position.DistanceBetween(chunk.Position, playerChunkPosition).AnySupTo(loadDistance))
            {
                LoadedChunks.Remove(chunk.Position);
                Chunk.SaveChunk(SaveDir, chunk);
                GameObject.Destroy(chunk.ChunkObject);
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

                    if (!IsChunkLoaded(tmp) && !chunksToLoad.Contains(tmp))
                        chunksToLoad.Enqueue(tmp);
                }
            }
        }
    }

    /// <summary>
    /// Force le rechargement des chunks.
    /// </summary>
    public void Reload(Position centerChunkPosition, bool save = true)
    {
        this.CenterChunkPosition = centerChunkPosition;

        if(save)
            SaveLoadedChunks();

        LoadedChunks.Clear();

        foreach (Chunk chunk in LoadedChunks.Values)
            GameObject.Destroy(chunk.ChunkObject);

        chunksToLoad.Clear();

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
                    LoadedChunks.Add(tmp, GetChunkFromFile(tmp));
                }
            }
        }
    }

    /// <summary>
    /// Envoie True si le chunk à la position spécifiée est chargé en mémoire.
    /// </summary>
    public bool IsChunkLoaded(Position ChunkPos)
    {
        return LoadedChunks.ContainsKey(ChunkPos);
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
        return Chunk.LoadChunk(this, chunkPos, Seed);
    }

    /// <summary>
    /// Sauvegarde le terrain.
    /// </summary>
    public void SaveLoadedChunks()
    {
        foreach (Chunk chunk in LoadedChunks.Values)
            Chunk.SaveChunk(SaveDir, chunk);
    }

}

