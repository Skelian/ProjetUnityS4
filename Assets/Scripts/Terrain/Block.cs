using UnityEngine;

public class Block
{
    public static int AIR_BLOCK_ID = 0;

    public BlockDef Definition { get; set; }
    public Chunk Chunk { get; private set; }
    public Position Position { get; private set; }

    public int ID
    {
        get
        {
            return Definition.Id;
        }
    }

    public Block(BlockDef definition, Position globalPosition, Chunk parent)
    {
        this.Definition = definition;
        this.Chunk = parent;
        this.Position = globalPosition;
    }

}
