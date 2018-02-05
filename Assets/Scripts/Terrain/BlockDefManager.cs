using System.Collections.Generic;

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

    public static void ImportBlockDef()
    {
        string path = Data.GetDataPath() + "/blocs.dat";


    }

    /// <summary>
    /// Retourne la définition correspondant à l'identifiant indiqué.
    /// </summary>
    public static BlockDef GetBlockDef(int blockID)
    {
        return blockDefs[blockID];
    }

}
