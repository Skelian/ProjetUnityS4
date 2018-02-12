using System;
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
                Reload(CenterChunk.Position, false);
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
        this.ParentSave = save;
        this.DimensionID = dimensionID;
        this.Seed = seed;
        this.loadDistance = loadDistance;

        saveDir = save.GetWorldDir(dimensionID) + "chunks/";
        if (!Directory.Exists(saveDir))
            Directory.CreateDirectory(saveDir);

        Reload(EntityUtils.GetChunkPosition(playerPosition), false);
    }

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    public void Update(Vector3 playerPosition)
    {
        Position playerChunkPosition = EntityUtils.GetChunkPosition(playerPosition);
        if (playerChunkPosition == CenterChunk.Position)
            return;

        Position offset = Position.OffsetBetween(CenterChunk.Position, playerChunkPosition);
        if((Math.Abs(offset.X) >= LoadedChunks.Length) || (Math.Abs(offset.Y) >= LoadedChunks.Length) || (Math.Abs(offset.Z) >= LoadedChunks.Length))
        {
            Reload(playerChunkPosition);
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
                        LoadedChunks[x, y, z] = GetChunkFromFile(start.Add(x, y, z));
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

        CenterChunk = LoadedChunks[loadDistance, loadDistance, loadDistance];
    }

    /// <summary>
    /// Force le rechargement des chunks.
    /// </summary>
    public void Reload(Position centerChunkPosition, bool save = true)
    {
        if(save)
            SaveLoadedChunks();

        int tmp = loadDistance * 2 + 1;
        LoadedChunks = new Chunk[tmp, tmp, tmp];

        Position start = centerChunkPosition.SubtractAll(loadDistance);

        int x, y, z;
        for (x = 0; x < tmp; x++)
            for (y = 0; y < tmp; y++)
                for (z = 0; z < tmp; z++)
                    LoadedChunks[x, y, z] = GetChunkFromFile(start.Add(x, y, z));

        CenterChunk = LoadedChunks[loadDistance, loadDistance, loadDistance];
    }

    /// <summary>
    /// Envoie True si le chunk à la position spécifiée est chargé en mémoire.
    /// </summary>
    public bool IsChunkLoaded(Position ChunkPos)
    {
        Position distance = Position.DistanceBetween(CenterChunk.Position, ChunkPos);
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
            return LoadedChunks[centerIndex + CenterChunk.Position.X - chunkPos.X, centerIndex + CenterChunk.Position.Y - chunkPos.Y, centerIndex + CenterChunk.Position.Z - chunkPos.Z];
        }
        else
        {
            if (deep)
                return GetChunkFromFile(chunkPos);
            else
                return null;
        }
    }

    public Chunk GetChunkFromFile(Position chunkPos)
    {
        return Chunk.LoadChunk(saveDir, chunkPos, Seed);
    }

    /// <summary>
    /// Sauvegarde le terrain.
    /// </summary>
    public void SaveLoadedChunks()
    {
        foreach(Chunk chunk in LoadedChunks)
            Chunk.SaveChunk(saveDir, chunk);
    }

}

