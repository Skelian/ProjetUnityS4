using System;
using System.IO;
using UnityEngine;

public class Chunk
{
    public const int SEED_FLAT = -1;

    /// <summary>
    /// Retourne un chunk vide (rempli d'air).
    /// </summary>
    public static Chunk NewEmptyChunk(Position chunkPos)
    {
        Block[,,] blocks = new Block[16, 16, 16];
        BlockDef definition = BlockDefManager.GetBlockDef(Block.DEFAULT_ID);
        Chunk chunk = new Chunk(chunkPos);

        int x, y, z;
        for (x = 0; x < 16; x++)
            for (y = 0; y < 16; y++)
                for (z = 0; z < 16; z++)
                    blocks[x, y, z] = new Block(definition, new Position(x, y, z), chunk);

        chunk.Blocks = blocks;
        return chunk;
    }

    /// <summary>
    /// Retourne un chunk généré selon la Seed spécifiée.
    /// </summary>
    public static Chunk CreateChunk(Position chunkPos, int seed)
    {
        Block[,,] blocks = new Block[16, 16, 16];
        Chunk chunk = new Chunk(chunkPos);

        if (seed == Chunk.SEED_FLAT)
        {
            BlockDef definition = BlockDefManager.GetBlockDef(1);
            int x, y, z;
            for (x = 0; x < 16; x++)
                for (y = 0; y < 16; y++)
                    for (z = 0; z < 16; z++)
                        blocks[x, y, z] = new Block(definition, new Position(x, y, z), chunk);

            chunk.Blocks = blocks;
            return chunk;
        }

        return null;
    }

    /// <summary>
    /// Coordonées du chunk.
    /// </summary>
    public Position position { get; private set; }

    /// <summary>
    /// Blocs du chunk.
    /// </summary>
    public Block[,,] Blocks { get; private set; }

    public Chunk(Block[,,] blocks, Position position)
    {
        this.Blocks = blocks;
        this.position = position;
    }

    private Chunk(Position position)
    {
        this.position = position;
    }

    /// <summary>
    /// Retourne le bloc présent aux coordonées locales indiquées (0 à 15).
    /// </summary>
    public Block GetBlock(int localXPos, int localYpos, int localZpos)
    {
        if (!IsValid(localXPos, localYpos, localZpos))
            return null;

        return Blocks[localXPos, localYpos, localZpos];
    }

    /// <summary>
    /// Remplace le bloc présent aux coordonées locales indiquées (0 à 15).
    /// </summary>
    public bool SetBlock(int id, int localXPos, int localYpos, int localZpos)
    {
        if (!IsValid(localXPos, localYpos, localZpos))
            return false;

        BlockDef definition = BlockDefManager.GetBlockDef(id);
        if (definition == null)
            return false;

        Blocks[localXPos, localYpos, localZpos].Definition = definition;
        return true;
    }

    /// <summary>
    /// Remplace les blocs présents entre les deux coordonées locales indiquées (0 à 15).
    /// </summary>
    public bool SetBlockBatch(int id, int localXpos_1, int localXpos_2, int localYpos_1, int localYpos_2, int localZpos_1, int localZpos_2)
    {
        if (!IsValid(localXpos_1, localXpos_2, localYpos_1, localYpos_2, localZpos_1, localZpos_2))
            return false;

        BlockDef definition = BlockDefManager.GetBlockDef(id);
        if (definition == null)
            return false;

        if (localXpos_1 > localXpos_2)
            Utils.Swap(ref localXpos_1, ref localXpos_2);

        if (localYpos_1 > localYpos_2)
            Utils.Swap(ref localYpos_1, ref localYpos_2);

        if (localZpos_1 > localZpos_2)
            Utils.Swap(ref localZpos_1, ref localZpos_2);

        int x, y, z;
        for (x = localXpos_1; x < localYpos_2; x++)
            for (y = localYpos_1; y < localYpos_2; y++)
                for (z = localZpos_1; z < localZpos_2; z++)
                    Blocks[x, y, z].Definition = definition;

        return true;
    }

    /// <summary>
    /// Remplace les blocs présents entre les deux coordonées locales indiquées (0 à 15).
    /// </summary>
    private bool SetBlockBatch(int id, Position first, Position second)
    {
        if (!IsValid(first, second))
            return false;

        BlockDef definition = BlockDefManager.GetBlockDef(id);
        if (definition == null)
            return false;

        Position.Smooth(first, second);

        int x, y, z;
        for (x = first.X; x < second.X; x++)
            for (y = first.Y; y < second.Y; y++)
                for (z = first.Z; z < second.Z; z++)
                    Blocks[x, y, z].Definition = definition;

        return true;
    }

    private bool IsValid(params int[] positions)
    {
        foreach (int pos in positions)
            if ((pos < 0) || (pos > 15))
                return false;

        return true;
    }

    public static bool SaveChunk(string saveFolder, Chunk chunk)
    {
        if (!saveFolder.EndsWith("/"))
            saveFolder += '/';

        if (!Directory.Exists(saveFolder))
            return false;

        string filePath = saveFolder + "[" + chunk.position.X + "." + chunk.position.Y + "." + chunk.position.Z + "].chunk";
        using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            int x, y, z;
            for (x = 0; x < 16; x++)
                for (y = 0; y < 16; y++)
                    for (z = 0; z < 16; z++)
                        writer.Write(chunk.Blocks[x, y, z].ID);
        }

        return true;
    }

    public static Chunk LoadChunk(string saveFolder, Position chunkPos, int seed)
    {
        if (!saveFolder.EndsWith("/"))
            saveFolder += '/';

        if (!Directory.Exists(saveFolder))
            return null;

        string filePath = saveFolder + "[" + chunkPos.X + "." + chunkPos.Y + "." + chunkPos.Z + "].chunk";

        if (!File.Exists(filePath))
            return CreateChunk(chunkPos, seed);

        var blocks = new Block[16, 16, 16];
        Chunk newChunk = new Chunk(chunkPos);

        using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            int x, y, z;
            for (x = 0; x < 16; x++)
                for (y = 0; y < 16; y++)
                    for (z = 0; z < 16; z++)
                        blocks[x, y, z] = new Block(BlockDefManager.GetBlockDef(reader.ReadInt32()), new Position(x, y, z), newChunk);
        }

        newChunk.Blocks = blocks;
        return newChunk;
    }

    private bool IsValid(params Position[] positions)
    {
        foreach (Position pos in positions)
            if (!IsValid(pos.X, pos.Y, pos.Z))
                return false;

        return true;
    }

}
