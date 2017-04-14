using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableData : ScriptableObject {

    public event System.Action onValuesUpdated;
    public bool autoUpdate;

    public void notifyOfUpdatedValues() {

        if(onValuesUpdated != null) {
            onValuesUpdated();
        }

    }

    protected virtual void OnValidate() {
        if(autoUpdate) {
            notifyOfUpdatedValues();
        }
    }

}
