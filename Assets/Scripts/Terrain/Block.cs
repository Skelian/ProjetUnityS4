using UnityEngine;

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

    public Position LocalPosition()
    {
        return new Position(Position.X % Chunk.CHUNK_SIZE, Position.Y % Chunk.CHUNK_SIZE, Position.Z % Chunk.CHUNK_SIZE);
    }

    public Block GetNearBlock(Utils.Face face)
    {
        Position pos = Position.NearPosition(face);
        Chunk chunk = ParentChunk.World.GetChunk(EntityUtils.ToChunkPosition(pos));
        return (chunk != null ? chunk.GetLocalBlock(EntityUtils.ToLocalChunkPosition(pos)) : null);
    }

}
