using UnityEngine;
using System.Collections;

//This simple script makes the editor preview objects invisible when you enter play mode
public class HideOnPlay : MonoBehaviour {

    void Start() {

        gameObject.SetActive(false);

    }

}
