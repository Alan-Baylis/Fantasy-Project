using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void drawTexture(Texture2D texture) {

        int width = texture.width;
        int height = texture.height;

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width, 1, height);

    }

    public void drawMesh(MeshData meshData, Texture2D texture) {

        meshFilter.sharedMesh = meshData.createMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;

    }

}
