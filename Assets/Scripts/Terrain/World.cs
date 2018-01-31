using System;
using System.Collections.Generic;
using System.IO;

class World
{
    /// <summary>
    /// Chunks chargés en mémoire.
    /// </summary>
    public Chunk[,,] LoadedChunks { get; private set; }

    /// <summary>
    /// Chunk central chargé en mémoire.
    /// </summary>
    public Chunk CenterChunk { get; private set; }

    /// <summary>
    /// Repertoire ou sont stockés les fichiers de chunks.
    /// </summary>
    private string saveDir;

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
                Reload(false);
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

    public World(Save save, int dimensionID, int loadDistance, Position centerChunkPosition)
    {
        this.ParentSave = save;
        this.DimensionID = dimensionID;
        this.loadDistance = loadDistance;

        saveDir = ParentSave.GetWorldsDir() + "/DIM_" + this.DimensionID + "/";
        if (Directory.Exists(saveDir))
            Directory.CreateDirectory(saveDir);

        Reload(false);
    }

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    public void Update(Position centerChunkPosition)
    {

    }

    /// <summary>
    /// Force le rechargement des chunks.
    /// </summary>
    public void Reload(bool save = true)
    {
        if(save)
            SaveLoadedChunks();

        int tmp = loadDistance * 2 + 1;
        LoadedChunks = new Chunk[tmp, tmp, tmp];

        int x, y, z;
        for (x = 0; x < tmp; x++)
            for (y = 0; y < tmp; y++)
                for (z = 0; z < tmp; z++)
                    LoadedChunks[x, y, z] = getChunkFromFile(new Position(x, y, z));

        tmp = loadDistance + 1;
        CenterChunk = LoadedChunks[tmp, tmp, tmp];
    }

    /// <summary>
    /// Envoie True si le chunk à la position spécifiée est chargé en mémoire.
    /// </summary>
    public bool IsChunkLoaded(Position ChunkPos)
    {
        var distance = Position.DistanceBetween(CenterChunk.position, ChunkPos);
        return ((distance.X <= loadDistance) && (distance.Y <= loadDistance) && (distance.Z <= loadDistance));
    }

    /// <summary>
    /// Retourne le chunk présent aux coordonées indiquées.
    /// </summary>
    /// <param name="deep">Recherche le chunk dans la sauvegarde s'il n'est pas chargé.</param>
    public Chunk GetChunk(Position chunkPos, bool deep = false)
    {
        if (IsChunkLoaded(chunkPos))
        {
            int centerIndex = loadDistance + 1;
            return LoadedChunks[centerIndex + CenterChunk.position.X - chunkPos.X, centerIndex + CenterChunk.position.Y - chunkPos.Y, centerIndex + CenterChunk.position.Z - chunkPos.Z];
        }
        else
        {
            if (deep)
                return getChunkFromFile(chunkPos);
            else
                return null;
        }
    }

    public Chunk getChunkFromFile(Position chunkPos)
    {
        return Chunk.LoadChunk(saveDir, chunkPos);
    }

    /// <summary>
    /// Sauvegarde le terrain.
    /// </summary>
    public void SaveLoadedChunks()
    {
        if (!Directory.Exists(saveDir))
            Directory.CreateDirectory(saveDir);

        int x, y, z;
        for (x = 0; x < loadDistance; x++)
            for (y = 0; y < loadDistance; y++)
                for (z = 0; z < loadDistance; z++)
                    Chunk.SaveChunk(saveDir, LoadedChunks[x, y, z]);
    }

}

