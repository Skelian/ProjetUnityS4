using System.Collections;
using System.Collections.Generic;

public class Chunk {
	
	public static Chunk newEmptyChunk(Position chunkPos) {
		Block[,,] blocks = new Block[16, 16, 16];
		BlockDef definition = BlockDefManager.getBlockDef (Block.DEFAULT_ID);
		Chunk chunk = new Chunk (chunkPos);

		for (int x = 0; x < 16; x++)
			for (int y = 0; y < 16; y++)
				for (int z = 0; z < 16; z++)
					blocks [x, y, z] = new Block (definition, new Position (x, y, z), chunk);

		chunk.Blocks = blocks;
		return chunk;
	}

	private Position position;
	private Block[,,] blocks;

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
		
	public Block[,,] Blocks {
		get {
			return this.blocks;
		}

		private set {
			this.blocks = value;
		}
	}

	public Chunk(Block[,,] blocks, Position position) {
		this.blocks = blocks;
		this.position = position;
	}

	private Chunk(Position position) {
		this.position = position;
	}
		
	public Block getBlock(int localXPos, int localYpos, int localZpos) {
		if (!isValid(localXPos, localYpos, localZpos))
			return null;

		return blocks [localXPos, localYpos, localZpos];
	}

	public bool setBlock(int id, int localXPos, int localYpos, int localZpos) {
		if (!isValid(localXPos, localYpos, localZpos))
			return false;

		BlockDef definition = BlockDefManager.getBlockDef (id);
		if (definition == null)
			return false;

		blocks [localXPos, localYpos, localZpos].Definition = definition;
		return true;
	}

	public bool setBlockBatch(int id, int localXpos_1, int localXpos_2, int localYpos_1, int localYpos_2, int localZpos_1, int localZpos_2) {
		if (!isValid (localXpos_1, localXpos_2, localYpos_1, localYpos_2, localZpos_1, localZpos_2))
			return false;

		BlockDef definition = BlockDefManager.getBlockDef (id);
		if (definition == null)
			return false;

		if (localXpos_1 > localXpos_2)
			Utils.Swap (ref localXpos_1, ref localXpos_2);

		if (localYpos_1 > localYpos_2)
			Utils.Swap (ref localYpos_1, ref localYpos_2);

		if (localZpos_1 > localZpos_2)
			Utils.Swap (ref localZpos_1, ref localZpos_2);
			
		int x, y, z;
		for (x = localXpos_1; x < localYpos_2; x++)
			for (y = localYpos_1; y < localYpos_2; y++)
				for (z = localZpos_1; z < localZpos_2; z++)
					blocks [x, y, z].Definition = definition;

		return true;
	}

	private bool isValid(params int[] positions) {
		foreach (int pos in positions)
			if ((pos < 0) || (pos > 15))
				return false;

		return true;
	}

	private bool isValid(params Position[] positions) {
		foreach (Position pos in positions)
			if (!isValid (pos.x, pos.y, pos.z))
				return false;

		return true;
	}

}
