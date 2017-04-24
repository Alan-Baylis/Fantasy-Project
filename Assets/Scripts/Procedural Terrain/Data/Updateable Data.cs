using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable objects can be created as a file in the editor, so updatable data serves as a parent class for all our
//other data objects, it allows any updates made in the objects to be conveyed to any listening objects
public class UpdateableData : ScriptableObject {

    //Events are like lists of functions that have been subscribed to this object
    public event System.Action onValuesUpdated;
    //Whether or not we auto call the onValuesUpdated() event or does it only happen when the user chooses
    public bool autoUpdate;

    //This function gets called by the Unity Editor whenever it is updated
    public void notifyOfUpdatedValues() {

        //Unsubscribe thhis function from the update event in UnityEditor, so we aren't subscribed multiple times
        UnityEditor.EditorApplication.update -= notifyOfUpdatedValues;

        //If there are functions to call in our event updater, call the functions/event
        if(onValuesUpdated != null) {
            onValuesUpdated();
        }

    }

    //OnValidate is called whenever a value is changed in an object in the editor
    protected virtual void OnValidate() {
        //If we want to auto update any changes made, then subscibe the notifyOfUpdatedValeus() function to the update event in the UnityEditor
        if(autoUpdate) {
            UnityEditor.EditorApplication.update += notifyOfUpdatedValues;
        }
    }

}
