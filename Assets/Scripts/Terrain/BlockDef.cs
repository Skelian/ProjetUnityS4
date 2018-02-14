using UnityEngine;

public class BlockDef
{
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
    /// Nombre de dégats infligés au joueur par 0.5 seconde au contact du bloc
    /// </summary>
    public int DamageOnContact { get; private set; }

    public BlockDef(int id, string name, int resistance, bool transparent, bool gravity, bool destructible, int damageOnContact)
    {
        Id = id;
        BlockName = name;
        Resistance = resistance;
        Transparent = transparent;
        Gravity = gravity;
        Destructible = destructible;
        DamageOnContact = damageOnContact;
    }

    public class BlockDefBuilder
    {
        private string blockName;
        private int id, resistance, damageOnContact = 0;
        private bool gravity = false, transparent = false, destructible = true;

        public BlockDefBuilder(int id, string blockName, int resistance)
        {
            this.id = id;
            this.blockName = blockName;
            this.resistance = resistance;
        }

        public BlockDefBuilder SetGravity(bool gravity)
        {
            this.gravity = gravity;
            return this;
        }

        public BlockDefBuilder SetDestructible(bool destructible)
        {
            this.destructible = destructible;
            return this;
        }

        public BlockDefBuilder SetTransparent(bool transparent)
        {
            this.transparent = transparent;
            return this;
        }

        public BlockDefBuilder SetDamageOnContact(int damageOnContact)
        {
            this.damageOnContact = damageOnContact;
            return this;
        }

        public BlockDef Build()
        {
            return new BlockDef(id, blockName, resistance, transparent, gravity, destructible, damageOnContact);
        }
    }

}
