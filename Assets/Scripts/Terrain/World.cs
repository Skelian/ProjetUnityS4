using System;
using System.Collections.Generic;
using System.IO;

public class World
{

    /// <summary>
    /// Seed de génération.
    /// </summary>
    public int Seed { get; private set; }

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

    public World(Save save, int dimensionID, int loadDistance, Position playerChunkPosition, int seed)
    {
        this.ParentSave = save;
        this.DimensionID = dimensionID;
        this.Seed = seed;
        this.loadDistance = loadDistance;

        Reload(false);
    }

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    public void Update(Position playerChunkPosition)
    {
        if (playerChunkPosition == CenterChunk.position)
            return;

        Position offset = Position.OffsetBetween(CenterChunk.position, playerChunkPosition);
        if((Math.Abs(offset.X) >= LoadedChunks.Length) || (Math.Abs(offset.Y) >= LoadedChunks.Length) || (Math.Abs(offset.Z) >= LoadedChunks.Length))
        {
            Reload();
            return;
        }

        int x, sx, off_x, end_x;
        int y, sy, off_y, end_y;
        int z, off_z, end_z;
        
        //setup X
        if(offset.X >= 0)
        {
            x = sx = 0;
            off_x = 1;
            end_x = LoadedChunks.Length;
        }
        else
        {
            x = sx = LoadedChunks.Length - 1;
            off_x = -1;
            end_x = -1;
        }

        //setup Y
        if (offset.Y >= 0)
        {
            y = sy = 0;
            off_y = 1;
            end_y = LoadedChunks.Length;
        }
        else
        {
            y = sy = LoadedChunks.Length - 1;
            off_y = -1;
            end_y = -1;
        }

        //setup Z
        if (offset.Z >= 0)
        {
            z = 0;
            off_z = 1;
            end_z = LoadedChunks.Length;
        }
        else
        {
            z = LoadedChunks.Length - 1;
            off_z = -1;
            end_z = -1;
        }

        Position start = playerChunkPosition.SubtractAll(loadDistance);
        int len = LoadedChunks.Length;

        while(z != end_z)
        {
            while(y != end_y)
            {
                while(x != end_x)
                {
                    if ((x + offset.X < 0) || (x + offset.X >= len) || (y + offset.Y < 0) || (y + offset.Y >= len) || (z + offset.Z < 0) || (z + offset.Z >= len))
                    {
                        LoadedChunks[x, y, z] = getChunkFromFile(start.Add(x, y, z));
                    }
                    else
                    {
                        if ((offset.X < 0 ? x - offset.X >= len : x - offset.X < 0) || (offset.Y < 0 ? y - offset.Y >= len : y - offset.Y < 0) || (offset.Z < 0 ? z - offset.Z >= len : z - offset.Z < 0))
                            Chunk.SaveChunk(saveDir, LoadedChunks[x, y, z]);

                        LoadedChunks[x, y, z] = LoadedChunks[x + offset.X, y + offset.Y, z + offset.Z];
                    }

                    x += off_x;
                }

                x = sx;
                y += off_y;
            }

            y = sy;
            z += off_z;
        }


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
        return Chunk.LoadChunk(saveDir, chunkPos, Seed);
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

