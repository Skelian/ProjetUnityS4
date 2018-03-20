using Simplex;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Chunk
{
    /// <summary>
    /// Dimensions d'un chunk.
    /// </summary>
    public const int CHUNK_SIZE = 16; //total volume = CHUNK_SIZE ^ 3

    /// <summary>
    /// Monde dans lequel se trouve le chunk.
    /// </summary>
    public World World { get; private set; }

    /// <summary>
    /// Coordonées du chunk.
    /// </summary>
    public Position Position { get; private set; }

    /// <summary>
    /// Vrai si le chunk est en train d'être sauvegardé.
    /// </summary>
    private bool beingSaved = false;

    /// <summary>
    /// Vrai si le chunk à été modifié depuis sa dernière sauvegarde
    /// </summary>
    private bool altered = true;

    /// <summary>
    /// Blocs du chunk, recalculation de la mesh lors du changement de blocs
    /// </summary>
    public Block[,,] Blocks { get; private set; }

    /// <summary>
    /// Fluides présent dans le chunk.
    /// </summary>
    private List<Fluid> fluids = new List<Fluid>();

    public static GameObject BaseChunkObject = Resources.Load("Prefabs/BaseChunkObject") as GameObject;
    public static void InitChunkObject()
    {
        Mesh mesh = new Mesh
        {
            vertices = new Vector3[] {
                    new Vector3 (0, 0, 0),
                    new Vector3 (16, 0, 0),
                    new Vector3 (16, 16, 0),
                    new Vector3 (0, 16, 0),
                    new Vector3 (0, 16, 16),
                    new Vector3 (16, 16, 16),
                    new Vector3 (16, 0, 16),
                    new Vector3 (0, 0, 16),
            },

            triangles = new int[] {
                    0, 2, 1,
                    0, 3, 2,
                    2, 3, 4,
                    2, 4, 5,
                    1, 2, 5,
                    1, 5, 6,
                    0, 7, 4,
                    0, 4, 3,
                    5, 4, 7,
                    5, 7, 6,
                    0, 6, 7,
                    0, 1, 6
            }
        };

        mesh.RecalculateBounds();
        BaseChunkObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    /// <summary>
    /// GameObject du chunk
    /// </summary>
    public GameObject ChunkObject { get; private set; }
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    public void Update()
    {
        bool updated = false;

        foreach (Fluid fluid in fluids)
            updated = (updated || fluid.Update());

        if(updated)
            RecalculateMesh();
    }

    public void Destroy()
    {
        if (ChunkObject != null)
            GameObject.Destroy(ChunkObject);

        ChunkObject = null;
    }

    public void Hide()
    {
        if (ChunkObject != null)
            ChunkObject.SetActive(false);
    }

    public void Instantiate()
    {
        if(ChunkObject != null)
        {
            ChunkObject.SetActive(true);
            return;
        }

        ChunkObject = GameObject.Instantiate(BaseChunkObject);

        ChunkObject.name = "chunk@" + Position.ToString();
        ChunkObject.transform.position = Position.MultAll(CHUNK_SIZE).ToVec3();
        ChunkObject.isStatic = true;

        meshFilter = ChunkObject.GetComponent<MeshFilter>();
        meshCollider = ChunkObject.GetComponent<MeshCollider>();
        RecalculateMesh();
    }

    private Chunk(World world, Position chunkPos)
    {
        Blocks = new Block[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        Position = chunkPos;
        World = world;
    }

    /// <summary>
    /// Recalcule la mesh du chunk.
    /// </summary>
    public void RecalculateMesh()
    {
        if (ChunkObject == null)
            return;

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                {
                    Block block = Blocks[x, y, z];

                    //passe le bloc s'il est vide
                    if (block.ID == Block.AIR_BLOCK_ID)
                        continue;

                    // haut
                    if (IsBlockTransparent(x, y + 1, z) || IsBlockScaled(x, y + 1, z))
                        BuildFace(block.Definition, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);

                    // bas
                    if (IsBlockTransparent(x, y - 1, z) || IsBlockScaled(x, y - 1, z))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);

                    // gauche
                    if (IsBlockTransparent(x - 1, y, z) || IsBlockScaled(x - 1, y, z))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris, true);

                    // droite
                    if (IsBlockTransparent(x + 1, y, z) || IsBlockScaled(x + 1, y, z))
                        BuildFace(block.Definition, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris, true);

                    // avant
                    if (IsBlockTransparent(x, y, z + 1) || IsBlockScaled(x, y, z + 1))
                        BuildFace(block.Definition, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris, true);

                    // arrière
                    if (IsBlockTransparent(x, y, z - 1) || IsBlockScaled(x, y, z - 1))
                        BuildFace(block.Definition, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris, true);

                }
            }
        }

        Mesh mesh = new Mesh
        {
            vertices = verts.ToArray(),
            uv = uvs.ToArray(),
            triangles = tris.ToArray()
        };

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    /// <summary>
    /// Construit une face d'un block du chunk.
    /// </summary>
    private void BuildFace(BlockDef definition, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris, bool applyScale = false)
    {
        int index = verts.Count;

        if (applyScale)
            up *= definition.Scale;

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

    /// <summary>
    /// Retourne vrai si le bloc indiqué est transparent, utilisé lors du calcul de la mesh du chunk.
    /// </summary>
    private bool IsBlockTransparent(int localPosX, int localPosY, int localPosZ)
    {
        return (Chunk.IsValid(localPosX, localPosY, localPosZ) ? Blocks[localPosX, localPosY, localPosZ].Definition.Transparent : true);
    }

    private bool IsBlockScaled(int localPosX, int localPosY, int localPosZ)
    {
        return (Chunk.IsValid(localPosX, localPosY, localPosZ) ? Blocks[localPosX, localPosY, localPosZ].Definition.Scale == 1 ? false : true : false);
    }

    /// <summary>
    /// Vrai si le block à la position indiqué est présent dans le chunk.
    /// </summary>
    public bool IsPresentInChunk(Position blockPos)
    {
        return (EntityUtils.ToChunkPosition(blockPos) == Position);
    }

    /// <summary>
    /// Retourne le block présent aux coordonées globales (si présent dans ce chunk).
    /// </summary>
    public Block GetBlock(Position blockPos)
    {
        return IsPresentInChunk(blockPos) ? Blocks[blockPos.X % CHUNK_SIZE, blockPos.Y % CHUNK_SIZE, blockPos.Z % CHUNK_SIZE] : null;
    }

    /// <summary>
    /// Retourne le bloc présent aux coordonées locales indiquées (0 à 15).
    /// </summary>
    public Block GetLocalBlock(int localXPos, int localYpos, int localZpos)
    {
        return (IsValid(localXPos, localYpos, localZpos) ? Blocks[localXPos, localYpos, localZpos] : null);
    }

    /// <summary>
    /// Remplace le bloc présent aux coordonées indiquées.
    /// </summary>
    public bool SetLocalBlock(int id, Position localBlockPos)
    {
        if (!IsValid(localBlockPos))
            return false;

        BlockDef definition = BlockDefManager.GetBlockDef(id);
        if (definition == null)
            return false;

        Blocks[localBlockPos.X, localBlockPos.Y, localBlockPos.Z].Definition = definition;

        altered = true;
        RecalculateMesh();

        return true;
    }

    /// <summary>
    /// Remplace les blocs présents entre les deux coordonées locales indiquées.
    /// </summary>
    public bool SetLocalBlockBatch(int id, Position first, Position second)
    {
        if (!IsValid(first, second))
            return false;

        BlockDef newDef = BlockDefManager.GetBlockDef(id);
        if (newDef == null)
            return false;

        Position.Smooth(first, second);

        for (int x = first.X; x <= second.X; x++)
            for (int y = first.Y; y <= second.Y; y++)
                for (int z = first.Z; z <= second.Z; z++)
                    Blocks[x, y, z].Definition = newDef;


        altered = true;
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

    public static bool IsValid(params int[] positions)
    {
        foreach (int pos in positions)
            if ((pos < 0) || (pos >= CHUNK_SIZE))
                return false;

        return true;
    }

    public static bool IsValid(params Position[] positions)
    {
        foreach (Position pos in positions)
            if (!IsValid(pos.X, pos.Y, pos.Z))
                return false;

        return true;
    }

    public static void SaveChunk(string saveFolder, Chunk chunk)
    {
        //si aucune modification apportée / si le chunk est déja en train d'être sauvegardé, pas la peine de sauvegarder.
        if (!chunk.altered || chunk.beingSaved)
            return;

        string filePath = saveFolder + "[" + chunk.Position.X + "." + chunk.Position.Y + "." + chunk.Position.Z + "].chunk";
        using (var writer = new BinaryWriter(File.Open(filePath, FileMode.Create, FileAccess.Write)))
        {
            int x, y, z, currentBlock = chunk.Blocks[0, 0, 0].ID, len = 0;
            for (x = 0; x < CHUNK_SIZE; x++)
            {
                for (y = 0; y < CHUNK_SIZE; y++)
                {
                    for (z = 0; z < CHUNK_SIZE; z++)
                    {
                        if (chunk.Blocks[x, y, z].ID == currentBlock)
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

        chunk.altered = false;
    }

    public static Chunk LoadChunk(World world, Position chunkPos)
    {
        if (!Directory.Exists(world.SaveDir))
            return null;

        string filePath = world.SaveDir + "[" + chunkPos.X + "." + chunkPos.Y + "." + chunkPos.Z + "].chunk";

        if (!File.Exists(filePath))
            return CreateChunk(world, chunkPos);

        Chunk newChunk = new Chunk(world, chunkPos);
        Position globalChunkPos = chunkPos.MultAll(CHUNK_SIZE);

        using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            int x = 0, y = 0, z = 0, len;
            bool filled = false;
            while (!filled)
            {
                len = reader.ReadInt32();
                BlockDef definition = BlockDefManager.GetBlockDef(reader.ReadInt32());

                while (len-- > 0)
                {
                    newChunk.Blocks[x, y, z] = new Block(definition, globalChunkPos.Add(x, y, z), newChunk);

                    if (definition.BlockType == BlockDef.TYPE_FLUID)
                        newChunk.fluids.Add(new Fluid(newChunk.Blocks[x, y, z]));

                    if (z >= 15)
                    {
                        z = 0;
                        if (y >= 15)
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

        newChunk.altered = false;
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
        BlockDef definition = BlockDefManager.GetBlockDef(blockID);
        Chunk chunk = new Chunk(world, chunkPos);
        Position globalChunkPos = chunkPos.MultAll(CHUNK_SIZE);

        int x, y, z;
        for (x = 0; x < CHUNK_SIZE; x++)
            for (y = 0; y < CHUNK_SIZE; y++)
                for (z = 0; z < CHUNK_SIZE; z++)
                    chunk.Blocks[x, y, z] = new Block(definition, globalChunkPos.Add(x, y, z), chunk);

        return chunk;
    }

    /// <summary>
    /// Retourne un chunk généré selon la Seed spécifiée.
    /// </summary>
    public static Chunk CreateChunk(World world, Position chunkPos)
    {
        if (world.Seed == World.SEED_EMPTY_WORLD)
            return NewEmptyChunk(world, chunkPos);

        if (world.Seed == World.SEED_TEST_WORLD)
            return chunkPos.Y <= 0 ? NewTestChunk(world, chunkPos) : NewEmptyChunk(world, chunkPos);

        return NewChunkUsingSeed(world, chunkPos);
    }

    private static Chunk NewTestChunk(World world, Position position)
    {
        Chunk chunk = new Chunk(world, position);
        Position globalChunkPos = position.MultAll(CHUNK_SIZE);

        int x, y, z;
        for (x = 0; x < CHUNK_SIZE; x++)
            for (y = 0; y < CHUNK_SIZE; y++)
                for (z = 0; z < CHUNK_SIZE; z++)
                {
                    float value = Random.value;
                    chunk.Blocks[x, y, z] = new Block(BlockDefManager.GetBlockDef(value > 0.3f ? value < 0.7f ? 1 : 2 : 102), globalChunkPos.Add(x, y, z), chunk);
                }

        chunk.RecalculateMesh();
        return chunk;
    }

    private static Chunk NewChunkUsingSeed(World world, Position position)
    {
        Chunk chunk = new Chunk(world, position);
        Position globalChunkPos = position.MultAll(CHUNK_SIZE);

        if (position.Y <= 0)
            SetBlocks3DNoise(globalChunkPos, chunk);
        else
            SetBlocks2DNoise(globalChunkPos, chunk, 16);

        SetBlockOreGen(chunk);

        chunk.RecalculateMesh();
        return chunk;
    }

    private static void SetBlocks2DNoise(Position globalChunkPos, Chunk chunk, int baseGlobalPosition)
    {
        int raise = globalChunkPos.Y - baseGlobalPosition + 90;
        float heightSmoother = 0.5f;

        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int z = 0; z < CHUNK_SIZE; z++)
            {
                Position blockPos = globalChunkPos.Add(x, 0, z);
                float noiseValue = Noise.CalcPixel2D(Mathf.Abs(blockPos.X), Mathf.Abs(blockPos.Z), 0.002f) * heightSmoother;
                for (int y = 0; y < CHUNK_SIZE; y++)
                    chunk.Blocks[x, y, z] = new Block(BlockDefManager.GetBlockDef(noiseValue > (raise + y) ? 1 : 0), blockPos.Add(0, y, 0), chunk);
            }
        }
    }

    private static void SetBlocks3DNoise(Position globalChunkPos, Chunk chunk)
    {
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    Position blockPos = globalChunkPos.Add(x, y, z);
                    float noiseValue = Noise.CalcPixel3D(blockPos.X, blockPos.Y, blockPos.Z, 0.015f);
                    chunk.Blocks[x, y, z] = new Block(BlockDefManager.GetBlockDef(noiseValue > 80 ? 2 : 0), blockPos, chunk);
                }
            }
        }
    }

    private static void SetBlockOreGen(Chunk chunk)
    {
        for (int x = 0; x < CHUNK_SIZE; x++)
        {
            for (int y = 0; y < CHUNK_SIZE; y++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    if (chunk.Blocks[x, y, z].ID != Block.AIR_BLOCK_ID)
                    {
                        if (Utils.rand.Next(1, 100) < 35)
                            return;

                        BlockDef ore = BlockDefManager.RandomBlock(BlockDef.TYPE_ORE);
                        chunk.Blocks[x, y, z].Definition = ore;

                        int gen = 50;

                        //down
                        SetBlockWithProbability(chunk, new Position(x + 1, y - 1, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y - 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y - 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y - 1, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y - 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y - 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y - 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y - 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y - 1, z + 1), ore, gen);

                        //up
                        SetBlockWithProbability(chunk, new Position(x + 1, y + 1, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y + 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y + 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y + 1, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y + 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y + 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y + 1, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y + 1, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y + 1, z + 1), ore, gen);

                        //sides
                        SetBlockWithProbability(chunk, new Position(x + 1, y, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y, z + 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x - 1, y, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x + 1, y, z), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y, z - 1), ore, gen);
                        SetBlockWithProbability(chunk, new Position(x, y, z + 1), ore, gen);
                    }
                }
            }
        }

    }

    private static void SetBlockWithProbability(Chunk chunk, Position position, BlockDef definition, int percent)
    {
        if (!IsValid(position))
            return;

        if (chunk.Blocks[position.X, position.Y, position.Z].ID != Block.AIR_BLOCK_ID)
            if (Utils.rand.Next(1, 100) > percent)
                chunk.Blocks[position.X, position.Y, position.Z].Definition = definition;
    }

}
