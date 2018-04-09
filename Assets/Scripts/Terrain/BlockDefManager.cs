using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockDefManager
{
    private static Dictionary<int, BlockDef> blockDefs = new Dictionary<int, BlockDef>();

    /// <summary>
    /// Ajoute un nouveau bloc au registre des définitions.
    /// </summary>
    public static bool AddBlockDef(BlockDef definition)
    {
        if (blockDefs.ContainsKey(definition.Id))
            return false;
        else
            blockDefs.Add(definition.Id, definition);

        return true;
    }

    public static void InitBlockDefinitions()
    {
        // les ids des blocs classiques commencent à 0
        AddBlockDef(new BlockDef.Builder(Block.AIR_BLOCK_ID, "air", 0)
            .SetTransparent(true)
            .Build());

        AddBlockDef(new BlockDef.Builder(1, "martian_stone", 2).Build());
        AddBlockDef(new BlockDef.Builder(2, "moon_stone", 2).Build());
        AddBlockDef(new BlockDef.Builder(3, "O2_block", 0).Build()); //resistance = 0 : se casse en un seul coup
        AddBlockDef(new BlockDef.Builder(4, "sand", 3)
            .SetGravity(true) //le sable est soumis à la gravitée
            .Build());

        //les ids des minéraux commencent à 100
        AddBlockDef(new BlockDef.Builder(100, "iron_ore", 3, BlockDef.TYPE_ORE).Build());
        AddBlockDef(new BlockDef.Builder(101, "titanium_ore", 4, BlockDef.TYPE_ORE).Build());
        AddBlockDef(new BlockDef.Builder(102, "uranium_ore", 4, BlockDef.TYPE_ORE).Build());
        AddBlockDef(new BlockDef.Builder(103, "oil_ore", 3, BlockDef.TYPE_ORE).Build());
        AddBlockDef(new BlockDef.Builder(104, "lead_ore", 3, BlockDef.TYPE_ORE).Build());

        //les ids des blocs techniques commencent à 200
        AddBlockDef(new BlockDef.Builder(200, "lava", 3, BlockDef.TYPE_FLUID)
            .SetDestructible(false)
            .SetDamageOnContact(6) //6pv de dégat par 0.5 seconde au contact de la lave
            .Build());

        AddBlockDef(new BlockDef.Builder(201, "lava", 3, BlockDef.TYPE_FLUID)
            .SetDestructible(false)
            .SetScale(0.75f)
            .SetDamageOnContact(6)
            .Build());

        AddBlockDef(new BlockDef.Builder(202, "lava", 3, BlockDef.TYPE_FLUID)
            .SetDestructible(false)
            .SetScale(0.50f)
            .SetDamageOnContact(6)
            .Build());

        AddBlockDef(new BlockDef.Builder(203, "lava", 3, BlockDef.TYPE_FLUID)
            .SetDestructible(false)
            .SetScale(0.25f)
            .SetDamageOnContact(6)
            .Build());
    }

    public static void BakeTextureAtlas()
    {
        List<Texture2D> textures = new List<Texture2D>();
        foreach (BlockDef definition in blockDefs.Values)
            textures.Add(Resources.Load("Textures/" + definition.BlockName) as Texture2D);

        Texture2D atlas = new Texture2D(0, 0);
        Rect[] rects = atlas.PackTextures(textures.ToArray(), 0);
        atlas.filterMode = FilterMode.Point;

        /** décommenter pour sauvegarder l'atlas en .png (debug)
        Texture2D nt = new Texture2D(atlas.width, atlas.height, TextureFormat.ARGB32, false);
        nt.SetPixels(0, 0, atlas.width, atlas.height, atlas.GetPixels());
        nt.Apply();
        File.WriteAllBytes("saves/atlas.png", nt.EncodeToPNG());
        **/

        int index = 0;
        foreach (BlockDef definition in blockDefs.Values)
            definition.UvRect = rects[index++];

        //apply texture atlas
        Chunk.BaseChunkObject.GetComponent<MeshRenderer>().sharedMaterial.SetTexture("_MainTex", atlas);
    }

    /// <summary>
    /// Retourne la définition correspondant à l'identifiant indiqué.
    /// </summary>
    public static BlockDef GetBlockDef(int blockID)
    {
        return blockDefs[blockID];
    }

    public static BlockDef RandomBlock(int blockType)
    {
        var matches = blockDefs.Where(entry => entry.Value.BlockType == blockType);
        return matches.ToList()[Utils.rand.Next(matches.Count())].Value;
    }

}
