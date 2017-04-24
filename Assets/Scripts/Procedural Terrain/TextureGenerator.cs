
using UnityEngine;

//This static class contains function to generate textures for the given height maps or colour maps
public static class TextureGenerator {

    //Genertes a coloured texture from the given colur map, the texture is of size width * height
    public static Texture2D textureFromColourMap(Color[] colourMap, int width, int height) {

        //Create a new blank texture of the given size
        Texture2D texture = new Texture2D(width, height);
        //Set each pixel in the texture to be well defined and not to blur with neighbouring pixels
        texture.filterMode = FilterMode.Point;
        //Set it so that the texture doesn't repeat at the edges
        texture.wrapMode = TextureWrapMode.Clamp;

        //Apply the pixels to the texture
        texture.SetPixels(colourMap);
        //Update the changes in the texture
        texture.Apply();

        //return the new texture
        return texture;

    }

    //Generate a black and white noise map from the inputed height map
    public static Texture2D textureFromHeightMap(float[,] heightMap) {

        //Get the dimensions from the array
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //Create a new colourmap to contain a colour for each coordinate in the map
        Color[] colourMap = new Color[width * height];
        //Iterate over the map
        for(int y = 0; y < height; y++) {
            for(int x = 0; x < width; x++) {

                //The coordinates of the colour map follow the layered rows and goes across each entry in the row, 
                //Heightmap values are between (0, 1) so choose a colour for the colour map that is haieghtMap[x, y]'s
                //fraction between black(0) & white(1)
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);

            }
        }

        //Create a texture for the heightmap and return it
        return textureFromColourMap(colourMap, width, height);

    }

}
