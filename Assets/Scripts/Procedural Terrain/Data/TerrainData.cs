using UnityEngine;
using System.Collections;

/// <summary>
/// Terrain Data contains information used to represent and edit the generated map in the game/editor
/// </summary>
[CreateAssetMenu()]
public class TerrainData : UpdateableData {

    /// <summary>
    /// Whether or not we want to use a generated falloff map on the meshes
    /// </summary>
    public bool useFalloffMap;
    /// <summary>
    /// Whether or not flatshading is to be implemented 
    /// </summary>
    public bool useFlatShading;

    /// <summary>
    /// The size of each mesh in the scene
    /// </summary>
    public float uniformScale = 10f;

    /// <summary>
    /// The amount that we scale up the mesh heights by
    /// </summary>
    public float meshHeightMultiplier;
    /// <summary>
    /// The curve function that adjusts all heights in the map
    /// </summary>
    public AnimationCurve meshHeightCurve;

    /// <summary>
    /// Get the minimum value that is possible in the map, assuming the minimum value is at 0
    /// </summary>
    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }

    /// <summary>
    /// Get the maximum value that is possible in the map, assuming the maximum value is at 1
    /// </summary>
    public float maxHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

}
