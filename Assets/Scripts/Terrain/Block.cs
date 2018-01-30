using System.Collections;
using System.Collections.Generic;

public class Block {

	public static int DEFAULT_ID = 0;

	private BlockDef definition;
	private Position position;
	private Chunk chunk;

	public int ID {
		get {
			return this.definition.ID;
		}
		set {
			this.definition = BlockDefManager.getBlockDef(value);
		}
	}

	public BlockDef Definition {
		get {
			return this.definition;
		}
		set {
			this.definition = value;
		}
	}

	public int X {
		get {
			return this.position.x;
		}
	}

	public int Y {
		get {
			return this.position.y;
		}
	}

	public int Z {
		get {
			return this.position.z;
		}
	}

	public Chunk Chunk {
		get {
			return this.chunk;
		}
	}

	public Block(BlockDef definition, Position localPosition, Chunk parent) {
		this.definition = definition;
		this.position = localPosition;
		this.chunk = parent;
	}

	public BlockDef getBlockDef() {
		return definition;
	}

	public Chunk getChunk() {
		return chunk;
	}

	public Position getAbsolutePosition() {
		return new Position (chunk.X * 16 + position.x, chunk.Y * 16 + position.y, chunk.Z * 16 + position.z);
	}

}
