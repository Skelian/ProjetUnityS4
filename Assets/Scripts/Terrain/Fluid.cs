public class Fluid  {

    public Block block;
    private Block West;
    private Block East;
    private Block North;
    private Block South;
    private Block Down;


    public Fluid(Block c)
    {
        block = c;
        West = block.GetNearBlock(Utils.Face.WEST);
        East = block.GetNearBlock(Utils.Face.EAST);
        North = block.GetNearBlock(Utils.Face.NORTH);
        South = block.GetNearBlock(Utils.Face.SOUTH);
        Down = block.GetNearBlock(Utils.Face.DOWN);
    }

    // Update is called once per frame
    public bool Update () {
        if ((Down.ID == Block.AIR_BLOCK_ID) || (Down.ID == 201) || (Down.ID == 202) || (Down.ID == 203))
        {
            Down.ParentChunk.SetLocalBlock(200, Down.Position);
            return true;
        }
        else
        {
            if ((Down.ID != 200) && (block.ID < 203))
            {

                if (West.ID == Block.AIR_BLOCK_ID)
                {
                    West.ParentChunk.SetLocalBlock(block.ID + 1, West.Position);
                }
                if (East.ID == Block.AIR_BLOCK_ID)
                {
                    East.ParentChunk.SetLocalBlock(block.ID + 1, East.Position);
                }
                if (North.ID == Block.AIR_BLOCK_ID)
                {
                    North.ParentChunk.SetLocalBlock(block.ID + 1, North.Position);
                }
                if (South.ID == Block.AIR_BLOCK_ID)
                {
                    South.ParentChunk.SetLocalBlock(block.ID + 1, South.Position);
                }

            }
        }

        return false;
    }
}
