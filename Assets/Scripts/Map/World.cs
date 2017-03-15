using AttributeAppl;
using UnityEngine;

public class World : MonoBehaviour {

    public string worldName;

    public Chunk[] loadedChunks;
    public Chunk[] renderedChunks;

    private void Start() {

        generateMap();

    }

    public void generateMap() {

        Biome hills = new Hills();

        TerrainGenerator terrainGenerator = new TerrainGenerator(this, hills);

        for(int i = 0; i < 4; i++) {
            for(int j = 0; j < 4; j++) {
                ChunkLocation chunkLocation = new ChunkLocation(this, i, j, 0);
                Chunk chunk = terrainGenerator.generateChunk(chunkLocation, Direction.NORTH);
                chunk.transform.parent = transform.parent;
                chunk.toggleRender();
            }
        }

    }

}