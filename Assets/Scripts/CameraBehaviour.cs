using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private RaycastHit ray;
    private Block lookingAt;
    public int raycastDistance = 5;

    GameObject selector;

    void Start()
    {
         selector = Instantiate(Resources.Load("Prefabs/Selector") as GameObject);
    }

    void Update()
    {
        if(lookingAt == null)
            return;

        if(Input.GetMouseButtonUp(0))
        {
            lookingAt.ParentChunk.SetLocalBlock(Block.AIR_BLOCK_ID, EntityUtils.ToLocalChunkPosition(lookingAt.Position));
            lookingAt.ParentChunk.RecalculateMesh();
            lookingAt = null;
        }
    }

	void FixedUpdate() {
        bool hit = Physics.Raycast(transform.position, transform.forward, out ray, raycastDistance);
        lookingAt = (hit ? WorldLoader.instance.GetWorld().GetBlock(Position.FromVec3(ray.point)) : null);

        if (lookingAt == null)
        {
            selector.SetActive(false);
        }
        else
        {
            selector.SetActive(true);
            selector.transform.position = new Vector3(lookingAt.Position.X + 0.5f, lookingAt.Position.Y - 0.5f, lookingAt.Position.Z + 0.5f);
        }
    }

    public Block GetBlockLookingAt()
    {
        return lookingAt;
    }
}
