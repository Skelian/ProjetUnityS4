public class Block
{

    public static int DEFAULT_ID = 0;

    public BlockDef Definition { get; set; }
    public Chunk Chunk { get; private set; }
    private Position position;

    public int ID
    {
        get
        {
            return Definition.Id;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe X (0 à 15).
    /// </summary>
    public int X
    {
        get
        {
            return position.X;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe Y (0 à 15).
    /// </summary>
    public int Y
    {
        get
        {
            return position.Y;
        }
    }

    /// <summary>
    /// Coordonée locale du bloc sur l'axe Z (0 à 15).
    /// </summary>
    public int Z
    {
        get
        {
            return position.Z;
        }
    }

    public Block(BlockDef definition, Position localPosition, Chunk parent)
    {
        this.Definition = definition;
        this.position = localPosition;
        this.Chunk = parent;
    }

    /// <summary>
    /// Retourne la position absolue du bloc dans le terrain.
    /// </summary>
    public Position getAbsolutePosition()
    {
        return new Position(Chunk.position.X * 16 + position.X, Chunk.position.Y * 16 + position.Y, Chunk.position.Z * 16 + position.Z);
    }

}
