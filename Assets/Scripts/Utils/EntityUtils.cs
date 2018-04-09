using UnityEngine;

class EntityUtils
{

    public static Position ToChunkPosition(Position entityPos)
    {
        return new Position(
                Mathf.FloorToInt(entityPos.X / (float)Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.Y / (float)Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.Z / (float)Chunk.CHUNK_SIZE)
            );
    }

    public static Position ToChunkPosition(Vector3 entityPos)
    {
        return new Position(
                Mathf.FloorToInt(entityPos.x / Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.y / Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.z / Chunk.CHUNK_SIZE)
            );
    }

    public static Position ToLocalChunkPosition(Position pos)
    {
        int x = pos.X % Chunk.CHUNK_SIZE;
        int y = pos.Y % Chunk.CHUNK_SIZE;
        int z = pos.Z % Chunk.CHUNK_SIZE;

        return new Position(
                x < 0 ? 16 + x : x,
                y < 0 ? 16 + y : y,
                z < 0 ? 16 + z : z
            );
    }

}

