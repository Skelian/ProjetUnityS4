public class Block
{
    public static int AIR_BLOCK_ID = 0;

    public BlockDef Definition { get; set; }
    public Chunk ParentChunk { get; private set; }
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
        Definition = definition;
        ParentChunk = parent;
        Position = globalPosition;
    }

    public Block getNearBlock(Utils.Face face)
    {
        Position pos = Position.NearPosition(face);
        Chunk chunk = ParentChunk.World.GetLoadedChunk(EntityUtils.GetChunkPosition(pos));
        return (chunk != null ? chunk.GetBlock(pos) : null);
    }

}
