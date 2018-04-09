using System;
using UnityEngine;

public class Position
{
    private int x, y, z;

    public Position(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }

    public Position SubtractAll(int sub)
    {
        return AddAll(-sub);
    }

    public Position AddAll(int sub)
    {
        return new Position(x + sub, y + sub, z + sub);
    }

    public Position Subtract(int x, int y, int z)
    {
        return Add(-x, -y, -z);
    }

    public Position Add(int x, int y, int z)
    {
        return new Position(this.x + x, this.y + y, this.z + z);
    }

    public Position Mult(int coefX, int coefY, int coefZ)
    {
        return new Position(this.x * coefX, this.y * coefY, this.z * coefZ);
    }

    public Position MultAll(int coef)
    {
        return new Position(this.x * coef, this.y * coef, this.z * coef);
    }

    public Position Div(int coefX, int coefY, int coefZ)
    {
        return new Position(this.x / coefX, this.y / coefY, this.z / coefZ);
    }

    public Position DivAll(int coef)
    {
        return new Position(this.x / coef, this.y / coef, this.z / coef);
    }

    public bool AnySupEqTo(int value)
    {
        return ((x >= value) || (y >= value) || (z >= value));
    }

    public bool AllSupEqTo(int value)
    {
        return ((x >= value) && (y >= value) && (z >= value));
    }

    public bool AnySupTo(int value)
    {
        return ((x > value) || (y > value) || (z > value));
    }

    public bool AllSupTo(int value)
    {
        return ((x > value) && (y > value) && (z > value));
    }

    public bool AnyInfEqTo(int value)
    {
        return ((x <= value) || (y <= value) || (z <= value));
    }

    public bool AllInfEqTo(int value)
    {
        return ((x <= value) && (y <= value) && (z <= value));
    }

    public bool AnyInfTo(int value)
    {
        return ((x < value) || (y < value) || (z < value));
    }

    public bool AllInfTo(int value)
    {
        return ((x < value) && (y < value) && (z < value));
    }

    public override string ToString()
    {
        return '[' + x.ToString() + ',' + y.ToString() + ',' + z.ToString() + ']';
    }

    public Position CopyOf()
    {
        return new Position(x, y, z);
    }

    public Position NearPosition(Utils.Face face)
    {
        Position pos = CopyOf();

        switch(face)
        {
            case Utils.Face.UP:
                ++pos.y;
                break;

            case Utils.Face.DOWN:
                --pos.y;
                break;

            case Utils.Face.NORTH:
                ++pos.x;
                break;

            case Utils.Face.SOUTH:
                --pos.x;
                break;

            case Utils.Face.EAST:
                ++pos.z;
                break;

            case Utils.Face.WEST:
                --pos.z;
                break;
        }

        return pos;
    }

    public static bool operator!=(Position pos1, Position pos2)
    {
        return !((pos1.x == pos2.x) && (pos1.y == pos2.y) && (pos1.z == pos2.z));
    }

    public static bool operator== (Position pos1, Position pos2)
    {
        return ((pos1.x == pos2.x) && (pos1.y == pos2.y) && (pos1.z == pos2.z));
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || (obj.GetType() != typeof(Position)))
            return false;

        var pos = obj as Position;
        return ((x == pos.x) && (y == pos.y) && (z == pos.z));
    }

    public override int GetHashCode()
    {
        return (x * 31 + y) * 31 + z;
    }

    /// <summary>
    /// Echange les coordonées des deux positions tel que toutes les coordonées
    /// de first soient inférieures aux coordonées de second
    /// </summary>
    public static void Smooth(Position first, Position second)
    {
        if (first.x > second.x)
            Utils.Swap(ref first.x, ref second.x);

        if (first.y > second.y)
            Utils.Swap(ref first.y, ref second.y);

        if (first.z > second.z)
            Utils.Swap(ref first.z, ref second.z);
    }

    /// <summary>
    /// Retourne la distance entre les deux points
    /// </summary>
    public static Position DistanceBetween(Position first, Position second)
    {
        return new Position(Math.Abs(first.x - second.x), Math.Abs(first.y - second.y), Math.Abs(first.z - second.z));
    }

    /// <summary>
    /// Retourne l'offset entre les deux positions
    /// </summary>
    public static Position OffsetBetween(Position first, Position second)
    {
        return new Position(second.x - first.x, second.y - first.y, second.z - first.z);
    }

    public static Position FromVec3(Vector3 vec)
    {
        return new Position(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y), Mathf.FloorToInt(vec.z));
    }

    public Vector3 ToVec3()
    {
        return new Vector3(x, y, z);
    }
}
