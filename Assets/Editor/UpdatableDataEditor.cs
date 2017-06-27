using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdateableData), true)]
public class UpdatableDataEditor : Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        UpdateableData updatableData = (UpdateableData) target;

        if(GUILayout.Button("Update")) {
            updatableData.notifyOfUpdatedValues();
            EditorUtility.SetDirty(target);
        }

    }

}