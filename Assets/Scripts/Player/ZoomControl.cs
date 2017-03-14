using UnityEngine;

public class ZoomControl : MonoBehaviour {
	
	public float zoomSize = 5;
	//Initialises the orthographic projection in unity.
	//The variables used below, allows easy editting in the editor
	public float zoomInLimit = 2f;
	public float zoomOutLimit = 5f;

	public float zoomPerScroll = 1f;
	//The amount the camera zooms in by per scroll.

	// Update is called once per frame
	void Update() {
		if (Input.GetAxis("Mouse ScrollWheel") > 0) {
			if (zoomSize > zoomInLimit) { //Sets a parameter for the amount the player can zoom in.
				zoomSize -= zoomPerScroll; //Sets the amount the camera would zoom out on one scroll wheel.
			}
            
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0) {
			if (zoomSize < zoomOutLimit) { //Sets a parameter for the amount the player can zoom out.
				zoomSize += zoomPerScroll; //Sets the amount the camera would zoom in on one scroll wheel.
			}
		}
		GetComponent<Camera>().orthographicSize = zoomSize;
	}
}
