using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains data used in generating each bit of mapData
/// </summary>
[CreateAssetMenu()]
public class NoiseData : UpdateableData {

    /// <summary>
    /// The normalize mode for the terrain: whether or not we use local min/max s or global estimates of them
    /// </summary>
    public ProceduralNoise.NormalizeMode normalizeMode;

    /// <summary>
    /// The scaling of the noise in the mapData
    /// </summary>
    public float noiseScale;

    /// <summary>
    /// The number of times we overlay layers onto the map
    /// </summary>
    public int octaves;
    /// <summary>
    /// The persistance of each octave is the realtive strength of each subsequent amplitude
    /// </summary>
    [Range(0, 1)]
    public float persistance;
    /// <summary>
    /// Lacunarity is how the frequency scales across octaves
    /// </summary>
    public float lacunarity;

    /// <summary>
    /// The seed that is used in the pseudo random number generator in the noise generator
    /// </summary>
    public int seed;
    /// <summary>
    /// The offset each mesh will be from the origin
    /// </summary>
    public Vector2 offset;

    /// <summary>
    /// OnValidate is called whenever a value changes in the editor
    /// </summary>
    protected override void OnValidate() {

        //Lacunarity must always be greater or = than 1
        if(lacunarity < 1) {
            lacunarity = 1;
        }
        //There can't be a negative amount of octaves
        if(octaves < 0) {
            octaves = 0;
        }

        //Call onValidate for the updateable parent
        base.OnValidate();

    }

}
