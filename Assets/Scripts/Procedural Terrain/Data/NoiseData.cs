using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
//Contains data used in generating each bit of mapData
public class NoiseData : UpdateableData {

    //The normalize mode for the terrain: whether or not we use local min/max s or global estimates of them
    public ProceduralNoise.NormalizeMode normalizeMode;

    //The scaling of the noise in the mapData
    public float noiseScale;

    //The number of times we overlay layers onto the map
    public int octaves;
    //The persistance of each octave is the realtive strength of each subsequent amplitude
    [Range(0, 1)]
    public float persistance;
    //Lacunarity is how the frequency scales across octaves
    public float lacunarity;

    //The seed that is used in the pseudo random number generator in the noise generator
    public int seed;
    //The offset each mesh will be from the origin
    public Vector2 offset;

    //OnValidate is called whenever a value changes in the editor
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
