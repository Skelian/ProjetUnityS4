using UnityEngine;

class EntityUtils
{
    /// <summary>
    /// Return the entity current chunk position
    /// </summary>
    public static Position GetChunkPosition(Vector3 entityPos)
    {
        return new Position(
                                Mathf.FloorToInt(entityPos.x / Chunk.CHUNK_SIZE),
                                Mathf.FloorToInt(entityPos.y / Chunk.CHUNK_SIZE),
                                Mathf.FloorToInt(entityPos.z / Chunk.CHUNK_SIZE)
                            );
    }

    public static Position GetChunkPosition(Position entityPos)
    {
        return new Position(
                        Mathf.FloorToInt(entityPos.X / Chunk.CHUNK_SIZE),
                        Mathf.FloorToInt(entityPos.Y / Chunk.CHUNK_SIZE),
                        Mathf.FloorToInt(entityPos.Z / Chunk.CHUNK_SIZE)
                    );
    }

    public static Position GetBlocToChunkPosition(Vector3 entityPos)
    {
        return new Position(
                Mathf.FloorToInt(entityPos.x % Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.y % Chunk.CHUNK_SIZE),
                Mathf.FloorToInt(entityPos.z % Chunk.CHUNK_SIZE)
            );
    }

}

