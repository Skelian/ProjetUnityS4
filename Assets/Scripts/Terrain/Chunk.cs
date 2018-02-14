using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chunk
{
    /// <summary>
    /// Monde dans lequel se trouve le chunk.
    /// </summary>
    public static World World { get; private set; }

    /// <summary>
    /// Dimensions d'un chunk.
    /// </summary>
    public static int CHUNK_SIZE = 16; //total volume = CHUNK_SIZE ^ 3

    /// <summary>
    /// Coordonées du chunk.
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// Blocs du chunk, recalculation de la mesh lors du changement de blocs
    /// </summary>
    public Block[,,] blocks;
    public Block[,,] Blocks
    {
        get
        {
            return blocks;
        }

        private set
        {
            blocks = value;
            RecalculateMesh();
        }
    }

    public static GameObject BaseChunkObject = Resources.Load("Prefabs/BaseChunkObject") as GameObject;

    /// <summary>
    /// GameObject du chunk
    /// </summary>
    public GameObject ChunkObject { get; private set; }

    /// <summary>
    /// Recalcule la mesh du chunk de façon synchrone.
    /// </summary>
    public void RecalculateMesh()
    {
        Mesh visualMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for(int x = 0; x < CHUNK_SIZE; x++)
        {
            for(int y = 0; y < CHUNK_SIZE; y++)
            {
                for(int z = 0; z < CHUNK_SIZE; z++)
                {
                    Block block = Blocks[x, y, z];

                    //passe le bloc s'il est vide
                    if (block.ID == Block.AIR_BLOCK_ID)
                        continue;

                    // haut
                    if (IsBlockTransparent(x, y + 1, z))
                        BuildFace(block.Definition, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);

                    // bas
                    if (IsBlockTransparent(x, y - 1, z))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);

                    // gauche
                    if (IsBlockTransparent(x - 1, y, z))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);

                    // droite
                    if (IsBlockTransparent(x + 1, y, z))
                        BuildFace(block.Definition, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);

                    // avant
                    if (IsBlockTransparent(x, y, z + 1))
                        BuildFace(block.Definition, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);

                    // arrière
                    if (IsBlockTransparent(x, y, z - 1))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
                }
            }
        }

        visualMesh.vertices = verts.ToArray();
        visualMesh.uv = uvs.ToArray();
        visualMesh.triangles = tris.ToArray();

        visualMesh.RecalculateBounds();
        visualMesh.RecalculateNormals();

        ChunkObject.GetComponent<MeshFilter>().mesh = visualMesh;
        ChunkObject.GetComponent<MeshCollider>().sharedMesh = visualMesh;
    }

    /// <summary>
    /// Construit une face d'un block du chunk.
    /// </summary>
    private void BuildFace(BlockDef definition, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Rect rect = definition.UvRect;

        uvs.Add(new Vector2(rect.xMin, rect.yMin));
        uvs.Add(new Vector2(rect.xMin, rect.yMax));
        uvs.Add(new Vector2(rect.xMax, rect.yMax));
        uvs.Add(new Vector2(rect.xMax, rect.yMin));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }

    }

    private Chunk(World world, Position chunkPos)
    {
        Position = chunkPos;
        World = world;
        ChunkObject = InstantiateChunkObject(chunkPos);
    }

    /// <summary>
    /// Retourne vrai si le bloc indiqué est transparent, utilisé lors du calcul de la mesh du chunk.
    /// </summary>
    private bool IsBlockTransparent(int localPosX, int localPosY, int localPosZ)
    {
        return (IsValid(localPosX, localPosY, localPosZ) ? Blocks[localPosX, localPosY, localPosZ].Definition.Transparent : true);
    }

    /// <summary>
    /// Retourne le bloc présent aux coordonées locales indiquées (0 à 15).
    /// </summary>
    public Block GetLocalBlock(int localXPos, int localYpos, int localZpos)
    {
        return (IsValid(localXPos, localYpos, localZpos) ? Blocks[localXPos, localYpos, localZpos] : null);
    }

    /// <summary>
    /// Remplace le bloc présent aux coordonées locales indiquées (0 à 15).
    /// </summary>
    public bool SetLocalBlock(int id, int localXPos, int localYpos, int localZpos)
    {
        if (!IsValid(localXPos, localYpos, localZpos))
            return false;

        BlockDef definition = BlockDefManager.GetBlockDef(id);
        if (definition == null)
            return false;

        Blocks[localXPos, localYpos, localZpos].Definition = definition;
        RecalculateMesh();
        return true;
    }

    /// <summary>
    /// Remplace les blocs présents entre les deux coordonées locales indiquées (0 à 15).
    /// </summary>
    public bool SetLocalBlockBatch(int id, int localXpos_1, int localXpos_2, int localYpos_1, int localYpos_2, int localZpos_1, int localZpos_2)
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

        RecalculateMesh();
        return true;
    }

    /// <summary>
    /// Remplace les blocs présents entre les deux coordonées locales indiquées (0 à 15).
    /// </summary>
    public bool SetLocalBlockBatch(int id, Position first, Position second)
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

        RecalculateMesh();
        return true;
    }

    public bool IsEmpty()
    {
        foreach (Block b in Blocks)
            if (b.ID != Block.AIR_BLOCK_ID)
                return false;

        return true;
    }

    /// <summary>
    /// Crée un nouveau GameObject de chunk
    /// </summary>
    private static GameObject InstantiateChunkObject(Position chunkPos)
    {
        GameObject chunkObject = GameObject.Instantiate(BaseChunkObject);

        chunkObject.name = "chunk@" + chunkPos.ToString();
        chunkObject.transform.position = chunkPos.MultAll(CHUNK_SIZE).ToVec3();
        chunkObject.isStatic = true;

        return chunkObject;
    }

    private static bool IsValid(params int[] positions)
    {
        foreach (int pos in positions)
            if ((pos < 0) || (pos >= CHUNK_SIZE))
                return false;

        return true;
    }

    private static bool IsValid(params Position[] positions)
    {
        foreach (Position pos in positions)
            if (!IsValid(pos.X, pos.Y, pos.Z))
                return false;

        return true;
    }

    public static bool SaveChunk(string saveFolder, Chunk chunk)
    {
        string filePath = saveFolder + "[" + chunk.Position.X + "." + chunk.Position.Y + "." + chunk.Position.Z + "].chunk";
        using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            writer.Write(CHUNK_SIZE);

            int x, y, z, currentBlock = chunk.Blocks[0, 0, 0].ID, len = 0;
            for (x = 0; x < CHUNK_SIZE; x++)
            {
                for (y = 0; y < CHUNK_SIZE; y++)
                {
                    for (z = 0; z < CHUNK_SIZE; z++)
                    {
                        if(chunk.Blocks[x, y, z].ID == currentBlock)
                        {
                            ++len;
                        }
                        else
                        {
                            writer.Write(len);
                            writer.Write(currentBlock);
                            len = 1;
                            currentBlock = chunk.Blocks[x, y, z].ID;
                        }
                    }
                }
            }

            writer.Write(len);
            writer.Write(currentBlock);
        }

        return true;
    }

    public static Chunk LoadChunk(World world, Position chunkPos, int seed)
    {
        if (!Directory.Exists(world.saveDir))
            return null;

        string filePath = world.saveDir + "[" + chunkPos.X + "." + chunkPos.Y + "." + chunkPos.Z + "].chunk";

        if (!File.Exists(filePath))
            return CreateChunk(world, chunkPos, seed);

        var blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        Chunk newChunk = new Chunk(world, chunkPos);
        Position globalChunkPos = chunkPos.MultAll(CHUNK_SIZE);

        using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            if (reader.ReadInt32() != CHUNK_SIZE)
                return CreateChunk(world, chunkPos, seed); //format de chunk incorrect

            int x = 0, y = 0, z = 0, len;
            bool filled = false;
            while(!filled)
            {
                len = reader.ReadInt32();
                BlockDef definition = BlockDefManager.GetBlockDef(reader.ReadInt32());

                while(len-- > 0)
                {
                    blocks[x, y, z] = new Block(definition, globalChunkPos.Add(x, y, z), newChunk);
                    if(z >= 15)
                    {
                        z = 0;
                        if(y >= 15)
                        {
                            y = 0;
                            if (x >= 15)
                            {
                                filled = true;
                                break;
                            }
                            ++x;
                        }
                        else
                        {
                            ++y;
                        }
                    }
                    else
                    {
                        ++z;
                    }
                }
            }
        }

        newChunk.Blocks = blocks;
        return newChunk;
    }

    /// <summary>
    /// Retourne un chunk vide (rempli d'air).
    /// </summary>
    public static Chunk NewEmptyChunk(World world, Position chunkPos)
    {
        return NewFilledChunk(world, chunkPos, Block.AIR_BLOCK_ID);
    }

    public static Chunk NewFilledChunk(World world, Position chunkPos, int blockID)
    {
        Block[,,] blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        BlockDef definition = BlockDefManager.GetBlockDef(blockID);
        Chunk chunk = new Chunk(world, chunkPos);
        Position globalChunkPos = chunkPos.MultAll(CHUNK_SIZE);

        int x, y, z;
        for (x = 0; x < CHUNK_SIZE; x++)
            for (y = 0; y < CHUNK_SIZE; y++)
                for (z = 0; z < CHUNK_SIZE; z++)
                    blocks[x, y, z] = new Block(definition, globalChunkPos.Add(x, y, z), chunk);

        chunk.Blocks = blocks;
        return chunk;
    }

    /// <summary>
    /// Retourne un chunk généré selon la Seed spécifiée.
    /// </summary>
    public static Chunk CreateChunk(World world, Position chunkPos, int seed)
    {
        if (seed == World.SEED_EMPTY_WORLD)
        {
            return NewEmptyChunk(world, chunkPos);
        }

        //test flat world
        if (seed == World.SEED_TEST_WORLD)
        {
            return chunkPos.Y == 0 ? NewFilledChunk(world, chunkPos, 1) : NewEmptyChunk(world, chunkPos);
        }

        //temporary
        return NewEmptyChunk(world, chunkPos);
    }

}
