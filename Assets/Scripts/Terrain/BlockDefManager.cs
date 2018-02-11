using System.Collections.Generic;
using UnityEngine;

public class BlockDefManager
{

    private static Dictionary<int, BlockDef> blockDefs = new Dictionary<int, BlockDef>();

    /// <summary>
    /// Ajoute un nouveau bloc au registre des définitions.
    /// </summary>
    public static bool NewBlockDef(int id, string name, int resistance, bool destructible = true, bool gravity = false)
    {
        if (blockDefs.ContainsKey(id))
        {
            return false;
        }
        else
        {
            blockDefs.Add(id, new BlockDef(id, name, gravity, resistance, destructible));
        }

        return true;
    }

    public static void InitBlockDefinitions()
    {
        BlockDef.BaseBlockObject = Resources.Load("Prefabs/BaseBlockObject") as GameObject;

        if (BlockDef.BaseBlockObject == null)
            Debug.Log("baseblock is null");

        NewBlockDef(Block.DEFAULT_ID, "air", 0, false);
        NewBlockDef(1, "dirt", 2);
    }

    /// <summary>
    /// Retourne la définition correspondant à l'identifiant indiqué.
    /// </summary>
    public static BlockDef GetBlockDef(int blockID)
    {
        return blockDefs[blockID];
    }

}
