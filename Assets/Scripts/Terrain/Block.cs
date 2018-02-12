using UnityEngine;

public class Block
{
    public static int DEFAULT_ID = 0;

    public BlockDef Definition { get; set; }
    public Chunk Chunk { get; private set; }
    private GameObject blockObject;

    public int ID
    {
        get
        {
            return Definition.Id;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe X (0 à 15).
    /// </summary>
    public int X
    {
        get
        {
            return (int) blockObject.transform.position.x;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe Y (0 à 15).
    /// </summary>
    public int Y
    {
        get
        {
            return (int) blockObject.transform.position.y;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe Z (0 à 15).
    /// </summary>
    public int Z
    {
        get
        {
            return (int) blockObject.transform.position.z;
        }
    }

    public Block(BlockDef definition, Vector3 localPosition, Chunk parent)
    {
        this.Definition = definition;
        this.Chunk = parent;

        if (definition.Id != Block.DEFAULT_ID)
        {
            blockObject = definition.Instantiate();
            blockObject.transform.position = localPosition;
        }
    }

    /// <summary>
    /// Retourne la position absolue du bloc dans le terrain.
    /// </summary>
    public Position GetAbsolutePosition()
    {
        return new Position(Chunk.Position.X * 16 + X, Chunk.Position.Y * 16 + Y, Chunk.Position.Z * 16 + Z);
    }

}
