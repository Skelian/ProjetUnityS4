﻿using System;
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

    public Position Substract(int x, int y, int z)
    {
        return Add(-x, -y, -z);
    }

    public Position Add(int x, int y, int z)
    {
        return new Position(this.x + x, this.y + y, this.z + z);
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

    public static Position Vec3ToPos(Vector3 vec)
    {
        return new Position(Convert.ToInt32(vec.x), Convert.ToInt32(vec.y), Convert.ToInt32(vec.z));
    }

    public Vector3 ToVec3()
    {
        return new Vector3(x, y, z);
    }
}