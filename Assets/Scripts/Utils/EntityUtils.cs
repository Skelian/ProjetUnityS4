using UnityEngine;

class EntityUtils
{
    public static Position GetChunkPosition(Vector3 entityPos)
    {
        return new Position(Mathf.FloorToInt(entityPos.x / 16), Mathf.FloorToInt(entityPos.y / 16), Mathf.FloorToInt(entityPos.z / 16));
    }

}

