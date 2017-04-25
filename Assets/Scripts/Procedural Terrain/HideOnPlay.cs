using UnityEngine;
using System.Collections;

/// <summary>
/// This script stores references to a plane and mesh GameObject that are used to preview the map in the editor while not in play mode
/// </summary>
public class HideOnPlay : MonoBehaviour {

    void Start() {

        gameObject.SetActive(false);

    }

}
