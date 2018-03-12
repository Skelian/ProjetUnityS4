public class Utils
{

    public static System.Random rand = new System.Random();

    public static void Swap<T>(ref T a, ref T b)
    {
        T tmp = a;
        a = b;
        b = tmp;
    }

    public enum Face
    {
        UP, DOWN, NORTH, EAST, WEST, SOUTH
    }

}
