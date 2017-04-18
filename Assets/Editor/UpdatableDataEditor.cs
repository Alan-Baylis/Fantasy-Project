using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdatableData), true)]
public class UpdatableDataEditor : Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        UpdatableData updatableData = (UpdatableData) target;

        if(GUILayout.Button("Update")) {
            updatableData.notifyOfUpdatedValues();
            EditorUtility.SetDirty(target);
        }

    }

}