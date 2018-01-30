using System.Collections;
using System.Collections.Generic;

public class BlockDef {
	private string blockName;
	private bool gravity;
	private bool destructible;
	private int id;

	public BlockDef(int id, string name, bool gravity, bool destructible) {
		this.id = id;
		this.blockName = name;
		this.gravity = gravity;
		this.destructible = destructible;
	}

	public string BlockName {
		get {
			return this.blockName;
		}
	}

	public bool Gravity {
		get {
			return this.gravity;
		}
	}

	public bool IsDestructible {
		get {
			return this.destructible;
		}
	}

	public int ID {
		get {
			return this.id;
		}
	}

}
