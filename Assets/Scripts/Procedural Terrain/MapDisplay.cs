using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script stores references to a plane and mesh GameObject that are used to preview the map in the editor while not in play mode
/// </summary>
public class MapDisplay : MonoBehaviour {

    //A reference to the renderer for the plane GameObject, used to render 2D textures
    public Renderer textureRenderer;
    //A reference to both the filter (for mesh shape) and renderer (mesh look) of the mesh GameObject
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    //This function applies the given texture to the plane GameObject
    public void drawTexture(Texture2D texture) {

        //Get the widt hand height of the texture
        int width = texture.width;
        int height = texture.height;

        //Set the texture of the plane's material to the given texture
        textureRenderer.sharedMaterial.mainTexture = texture;
        //Set the scale of the plane to be the size of the texture along the x-z plane
        textureRenderer.transform.localScale = new Vector3(width, 1, height);

    }

    /// <summary>
    /// The function applies the mesh from meshData to the preview mesh GameObject in the editor
    /// </summary>
    /// <param name="meshData"></param>
    public void drawMesh(MeshData meshData) {

        //Generate the mesh from the meshData information and apply that mesh to the mesh GameObject, the reason we don't generate the actual mesh
        //earlier is that the mesh can only be created in the main thread while meshData is generated in a seperate thread.
        meshFilter.sharedMesh = meshData.createMesh();

        //Scale the mesh in all directions by the scaling factor of the terrainData
        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;

    }

}
