using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MapDisplay1 : MonoBehaviour {

    private Renderer textureRenderer;

    private void Start() {
        textureRenderer = GetComponent<Renderer>();
    }

    public void drawMap(int width, int height, Chunk[] loadedChunks) {

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * Chunk.chunkSize * height * Chunk.chunkSize];
        for(int x = 0; x < width; x++) {
            for(int z = 0; z < height; z++) {

                Chunk chunk = loadedChunks[z * width + x];
                int i = 0;
                foreach(ChunkColumn chunkColumn in chunk.getColumns()) {

                    Sprite blockSprite = chunkColumn.getHighestRenderableBlock().getSprite(Direction.NORTH);
                    Texture2D blockTexture = blockSprite.texture;
                    Color colour = blockTexture.GetPixel(0, 0);

                    Debug.Log(colour + " @ " + (z * width * Chunk.chunkSize + x + i));

                    colourMap[z * width * Chunk.chunkSize + x + i] = colour;

                    i++;
                }
                //colourMap[z * width + x] = Color.white;

            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;

        transform.localScale = new Vector3(width * Chunk.chunkSize, 0, height * Chunk.chunkSize);

    }

}
