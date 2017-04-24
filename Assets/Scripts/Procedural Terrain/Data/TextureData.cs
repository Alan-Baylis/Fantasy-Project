using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Create asset menu allows us to create a new TextureData from right clicking in the inspector
[CreateAssetMenu()]
//TextureData inherits from UpdatabelData so that it can notify objects when a value is changed in it,
//It represents data used for texturing the meshes and shader in each chunk 
public class TextureData : UpdateableData {

    //The size of a texture in the this data object
    const int textureSize = 512;
    //The encoding format a texture must be: this one is 16bit RGB
    const TextureFormat textureFormat = TextureFormat.RGB565;

    //An array of all textureLayers for the shader
    public Layer[] layers;

    //The current max and min heights in the map
    float savedMinHeight;
    float savedMaxHeight;

    //Apply the textures and data stored in this intance to the given material's shader
    public void applyToMaterial(Material material) {

        //Set the number of layers in the shader to our array size
        material.SetInt("layerCount", layers.Length);
        //Use Linq to select all tints from aevery entry in the layers array and save it to the baseColours array in the shader
        material.SetColorArray("baseColours", layers.Select(x => x.tint).ToArray());
        //Do the same linq jaron but save the startHeights for each layer to the baseStartHeights array in the shader
        material.SetFloatArray("baseStartHeights", layers.Select(x => x.startHeight).ToArray());
        //Save the blendStrengths for each layer to the baseBlends array in the shader
        material.SetFloatArray("baseBlends", layers.Select(x => x.blendStrength).ToArray());
        //... tintStrengths for each layer to the baseColourStrength
        material.SetFloatArray("baseColourStrength", layers.Select(x => x.tintStrength).ToArray());
        //..use your imagination..
        material.SetFloatArray("baseTextureScales", layers.Select(x => x.textureScale).ToArray());

        //Create a texture array from each texture in the layers and save it into the shader
        Texture2DArray texturesArray = generateTextureArray(layers.Select(x => x.texture).ToArray());
        material.SetTexture("baseTextures", texturesArray);

        //Update all the mesh heights in the material
        updateMeshHeights(material, savedMinHeight, savedMaxHeight);

    }

    //The function simply saves data to the shader variables and updates the saved values in this object aswell
    public void updateMeshHeights(Material material, float minHeight, float maxHeight) {

        material.SetFloat("minHeight", minHeight);
        material.SetFloat("maxHeight", maxHeight);

        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

    }

    //Converts a flat texture2D array to a texture2d array object that can be interpreted by the shader
    private Texture2DArray generateTextureArray(Texture2D[] textures) {

        //Create a texture array where every texture has dimensions exture size, the array has length textures.length, each texture is encoded
        //with the textureFormat encoding, and the textures allow mip mapping 
        Texture2DArray textureArray = new Texture2DArray(textureSize, textureSize, textures.Length, textureFormat, true);
        //Iterate over all the given textures and set them in order into the array at the correct index
        for(int i = 0; i < textures.Length; i++) {
            textureArray.SetPixels(textures[i].GetPixels(), i);
        }
        //Reflect the change in texture in the array 
        textureArray.Apply();

        //return the newly generated array object
        return textureArray;

    }

    //Layers can be edited in the editor
    [Serializable]
    //They represent a texture and all the colours, blends and tints that can be applied to that texture in the shader
    public class Layer {

        //The texture for this layer
        public Texture2D texture;
        //This size of the texture on the mesh
        public float textureScale;
        //The colour tint applied to the texture
        public Color tint;
        //The relative strength of that tint, 1 being max and min no strength
        [Range(0, 1)]
        public float tintStrength;
        //Where abouts in the map is the texture going to start to appear, the height of the map is in the range (0, 1)
        [Range(0, 1)]
        public float startHeight;
        //The fuzziness of the border between two textures, 1 = lots of mixing and 0 = no mixing (hard edge)
        [Range(0, 1)]
        public float blendStrength;

    }

}
