using System;
using System.Collections.Generic;

class World
{
    /// <summary>
    /// Chunks chargés en mémoire.
    /// </summary>
    public Chunk[,,] LoadedChunks { get; private set; }
    private Chunk center;
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
                Update(true);
            }
        }
    }

    /// <summary>
    /// Met à jour les chunks chargés.
    /// </summary>
    /// <param name="force">Force le rechargement des chunks.</param>
    public void Update(bool force = false)
    {
        if (force)
        {
            int tmp = loadDistance * 2 + 1;
            LoadedChunks = new Chunk[tmp, tmp, tmp];

            tmp = loadDistance + 1;
            center = LoadedChunks[tmp, tmp, tmp];
        }
    }

    /// <summary>
    /// Retourne le chunk central parmis les chunks chargés en mémoire
    /// </summary>
    public Chunk GetLastCenterChunk()
    {
        return center;
    }

    public bool IsChunkLoaded(Position ChunkPos)
    {
        var distance = Position.DistanceBetween(center.position, ChunkPos);
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
            return LoadedChunks[centerIndex + center.position.X - chunkPos.X, centerIndex + center.position.Y - chunkPos.Y, centerIndex + center.position.Z - chunkPos.Z];
        }
        else
        {
            if(deep)
            {
                ///temporaire
                return Chunk.newEmptyChunk(chunkPos);
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Sauvegarde le terrain.
    /// </summary>
    public void Save()
    {

    }

}

