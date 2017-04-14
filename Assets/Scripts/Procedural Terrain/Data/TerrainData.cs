using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TerrainData : UpdatableData {

    public bool useFalloffMap;
    public bool useFlatShading;

    public float uniformScale = 10f;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

}
