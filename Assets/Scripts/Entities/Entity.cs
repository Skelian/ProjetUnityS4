using UnityEngine;

abstract class Entity
{
    public Vector3 Position { get; set; }


    public Position GetChunkPosition(Vector3 entityPos)
    {
        return new Position(Mathf.FloorToInt(Position.x / 16), Mathf.FloorToInt(Position.y / 16), Mathf.FloorToInt(Position.z / 16));
    }

}

