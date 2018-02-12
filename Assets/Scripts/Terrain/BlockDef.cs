using UnityEngine;

public class BlockDef
{
    private static GameObject BaseBlockObject = Resources.Load("Prefabs/BaseBlockObject") as GameObject;

    /// <summary>
    /// GameObject du bloc, avec sa texture, un appel de la property renvoi un clone du bloc
    /// </summary>
    private GameObject blockObject;
    public GameObject BlockObject
    {
        get
        {
            return GameObject.Instantiate(blockObject);
        }
    }


    /// <summary>
    /// Nom du bloc.
    /// </summary>
    public string BlockName { get; private set; }

    /// <summary>
    /// True si le bloc est soumis à la gravité.
    /// </summary>
    public bool Gravity { get; private set; }

    /// <summary>
    /// True si le bloc est destructible.
    /// </summary>
    public bool Destructible { get; private set; }

    /// <summary>
    /// Identifiant du bloc.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Resistance à la destruction
    /// </summary>
    public int Resistance { get; private set; }

    public BlockDef(int id, string name, bool gravity, int resistance, bool destructible)
    {
        this.Id = id;
        this.BlockName = name;
        this.Gravity = gravity;
        this.Destructible = destructible;
        this.Resistance = resistance;

        if(id != Block.DEFAULT_ID)
            this.blockObject = GameObject.Instantiate(BaseBlockObject);
    }

}
