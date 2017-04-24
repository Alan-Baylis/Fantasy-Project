using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
//Terrain Data contains information used to represent and edit the generated map in the game/editor
public class TerrainData : UpdateableData {

    //Whether or not we want to use a generated falloff map on the meshes
    public bool useFalloffMap;
    //Whether or not flatshading is to be implemented 
    public bool useFlatShading;

    //The size of each mesh in the scene
    public float uniformScale = 10f;

    //The amount that we scale up the mesh heights by
    public float meshHeightMultiplier;
    //The curve function that adjusts all heights in the map
    public AnimationCurve meshHeightCurve;

    //Get the minimum value that is possible in the map, assuming the minimum value is at 0
    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }

    //Get the maximum value that is possible in the map, assuming the maximum value is at 1
    public float maxHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

}
