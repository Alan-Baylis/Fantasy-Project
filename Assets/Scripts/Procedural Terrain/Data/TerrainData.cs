using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class TerrainData : UpdatableData {

    public bool useFalloffMap;
    public bool useFlatShading;

    public float uniformScale = 10f;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }

    public float maxHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(1);
        }
    }

}
