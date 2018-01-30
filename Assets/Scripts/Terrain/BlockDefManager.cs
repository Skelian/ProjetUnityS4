using System.Collections;
using System.Collections.Generic;

public class BlockDefManager {

	private static Dictionary<int, BlockDef> blockDefs = new Dictionary<int, BlockDef>();

	public static bool newBlockDef(int id, string name, bool destructible = true, bool gravity = false) {
		if (blockDefs.ContainsKey (id)) {
			return false;
		} else {
			blockDefs.Add(id, new BlockDef(id, name, gravity, destructible));
		}

		return true;
	}

	public static BlockDef getBlockDef(int blockID) {
		return blockDefs [blockID];
	}

}
