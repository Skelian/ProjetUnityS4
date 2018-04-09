using UnityEngine;

public class BlockDef
{
    public const int TYPE_CLASSIC = 0;
    public const int TYPE_ORE = 1;
    public const int TYPE_FLUID = 2;

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

    /// <summary>
    /// Vrai si le bloc est transparent
    /// </summary>
    public bool Transparent { get; private set; }

    public Rect UvRect { get; set; }

    /// <summary>
    /// Epaisseur du bloc.
    /// </summary>
    public float Scale { get; private set; }

    /// <summary>
    /// Type du block.
    /// </summary>
    public int BlockType { get; private set; }

    /// <summary>
    /// Nombre de dégats infligés au joueur par 0.5 seconde au contact du bloc
    /// </summary>
    public int DamageOnContact { get; private set; }

    public BlockDef(int id, string name, int resistance, bool transparent, bool gravity, bool destructible, int damageOnContact, int blockType, float scale)
    {
        Id = id;
        BlockName = name;
        Resistance = resistance;
        Transparent = transparent;
        Gravity = gravity;
        Destructible = destructible;
        DamageOnContact = damageOnContact;
        BlockType = blockType;
        Scale = scale;
    }

    public class Builder
    {
        private string blockName;
        private int id, resistance, damageOnContact = 0, blockType;
        private bool gravity = false, transparent = false, destructible = true;
        private float scale = 1;

        public Builder(int id, string blockName, int resistance, int blockType = TYPE_CLASSIC)
        {
            this.id = id;
            this.blockName = blockName;
            this.resistance = resistance;
            this.blockType = blockType;
        }

        public Builder SetGravity(bool gravity)
        {
            this.gravity = gravity;
            return this;
        }

        public Builder SetDestructible(bool destructible)
        {
            this.destructible = destructible;
            return this;
        }

        public Builder SetTransparent(bool transparent)
        {
            this.transparent = transparent;
            return this;
        }

        public Builder SetDamageOnContact(int damageOnContact)
        {
            this.damageOnContact = damageOnContact;
            return this;
        }

        public Builder SetScale(float scale)
        {
            this.scale = scale;
            return this;
        }

        public BlockDef Build()
        {
            return new BlockDef(id, blockName, resistance, transparent, gravity, destructible, damageOnContact, blockType, scale);
        }
    }

}
