using UnityEngine;

class EntityUtils
{

    public static Position ToChunkPosition(Position entityPos)
    {
        return new Position(
                Mathf.FloorToInt(entityPos.X / Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.Y / Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.Z / Chunk.CHUNK_SIZE)
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

}

