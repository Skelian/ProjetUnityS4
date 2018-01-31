using System;

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
}
