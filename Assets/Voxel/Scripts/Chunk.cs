using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

//This class represents a chunk which is a 16 * 16 * 16 meshColliderection of Blocks in the World,
//it combines all these blocks together as one GameObject to save on rendering and lag
public class Chunk : MonoBehaviour {
    //The 3d array to store all blocks in the chunk
    public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
    //The chunkSize, it defaults to 16
    public static int chunkSize = 16;
    //Wheteher the chunk needs to update it's rendering
    public bool update = false;
    //Wheteher or not the chunk is loaded in the scene
    public bool rendered;

    //The skin of the chunk onto which the texture is rendered
    MeshFilter meshFilter;
    //The same idea as filter, but handles collisions
    MeshCollider meshCollider;
    //A reference to teh World the chunk is in 
    public World world;
    //It's position in the world
    public WorldPosition pos;

    //On start uo, get a reference to the instantiated MeshFilter and Collider
    void Start() {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
    }

    //Update is called once per frame
    void Update() {
        //If the chuunk needs to update it's rendering for this frame, do so
        if(update) {
            //Set update to false since we're up to date
            update = false;
            UpdateChunk();
        }
    }

    //Get the block at the coordinate in the chunk
    public Block getBlock(int x, int y, int z) {
        //Make sure that this coordinate is actually in this chunk
        if(inRange(x) && inRange(y) && inRange(z))
            //Get the block from the array
            return blocks[x, y, z];
        //If it's not in the chunk, get the block from the world i.e from another chunk
        return world.getBlock(pos.x + x, pos.y + y, pos.z + z);
    }

    //The function just makes sure that the given index isn't 0, or not out of range for the chunkSize
    public static bool inRange(int index) {
        if(index < 0 || index >= chunkSize)
            return false;

        return true;
    }

    //Set the block at the position to the given Block type
    public void setBlock(int x, int y, int z, Block block) {
        //Make sure that the coordinates are in the chunk
        if(inRange(x) && inRange(y) && inRange(z)) {
            //Assign the value in the array to the given block
            blocks[x, y, z] = block;
        } else {
            //If the coordinates are not in the chunk, set the block via the world 
            world.setBlock(pos.x + x, pos.y + y, pos.z + z, block);
        }
    }

    //Iterates through every block in the chunk and sets it's modified value to false;
    public void setBlocksUnmodified() {
        foreach(Block block in blocks) {
            block.changed = false;
        }
    }

    // Updates the chunk based on its contents
    void UpdateChunk() {
        //The chunk is rendered if it needs to be updated
        rendered = true;
        //Create a new instance of the MeshData class for rendering
        MeshData meshData = new MeshData();

        //Iterate over every coordinate in the chunk
        for(int x = 0; x < chunkSize; x++) {
            for(int y = 0; y < chunkSize; y++) {
                for(int z = 0; z < chunkSize; z++) {
                    //Add the data for the block at this coordinate to the meshData
                    meshData = blocks[x, y, z].blockData(this, x, y, z, meshData);
                }
            }
        }
        //render the mesh
        renderMesh(meshData);
    }

    // Sends the calculated mesh information
    // to the mesh and collision components
    void renderMesh(MeshData meshData) {
        //Reset the current mesh
        meshFilter.mesh.Clear();
        //Set the vertices to the vertices from meshData
        meshFilter.mesh.vertices = meshData.vertices.ToArray();
        //Set the triangles for the mesh the same way (triangles represent connections between vertices)
        meshFilter.mesh.triangles = meshData.triangles.ToArray();

        //Set the texture map in the same way and make it recalculate it's rendering with the cahnges implemented
        meshFilter.mesh.uv = meshData.uv.ToArray();
        meshFilter.mesh.RecalculateNormals();

        //Clear the mesh for the collider
        meshCollider.sharedMesh = null;
        //Make a new empty mesh
        Mesh mesh = new Mesh();
        //Set the vertices of the mesh to the meshData ones
        mesh.vertices = meshData.colVertices.ToArray();
        //Set the triangles in the same way
        mesh.triangles = meshData.colTriangles.ToArray();
        //Recalculate the shape of the collider with these changes
        mesh.RecalculateNormals();

        //Assign the new mesh to the collider
        meshCollider.sharedMesh = mesh;
    }

}