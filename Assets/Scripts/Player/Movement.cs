using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 1f; //Sets the initial speed of the object. **Can be changed later.
	Animator anim;
	void Start () 
	{
		anim = GetComponent<Animator> ();
	}

    // Update is called once per frame
    void Update() {

        //Get the key pressed and multiply it by the speed modifier, also smooth the movement with deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //The position is Vector3 so we ignore z by setting it to 0
        Vector3 movement = new Vector3(horizontal, vertical, 0f);
        //Add it to the current position
        this.transform.position += movement;

       if (horizontal > 0) {
            anim.SetInteger("State", -1);
            if (horizontal == 0)
                anim.SetInteger("State", 5);
            // else
            // anim.SetInteger("State", -1);
        } else if (horizontal < 0) {
            anim.SetInteger("State", 4);
            if (horizontal == 0)
                anim.SetInteger("State", 1);
            //else
            // anim.SetInteger("State", 4);
        }
        if (vertical > 0) {
            anim.SetInteger("State", 2);
            if (vertical == 0)
                anim.SetInteger("State", 0);
            // else
            // anim.SetInteger("State", 2);
        } else if (vertical < 0) {
            anim.SetInteger("State", 6);
            if (vertical == 0)
                anim.SetInteger("State", 3);
            //else
            // anim.SetInteger("State", 6);
        }
        if (horizontal == 0 && vertical == 0) {
            anim.SetInteger("State", 0);
        }



















        //See the difference?
        //        //if(Input.GetAxis("Horizontal")) This can be used instead of GetKey, as it allows you to change the keys in Unity.
        //		if (Input.GetKey (KeyCode.W)) 
        //		{    
        //			transform.position += new Vector3 (0.0f, speed * Time.deltaTime, 0.0f);
        //			anim.SetInteger ("State", 2);
        //		}
        //		if (Input.GetKey (KeyCode.S)) 
        //		{  
        //			transform.position -= new Vector3 (0.0f, speed * Time.deltaTime, 0.0f);
        //			anim.SetInteger ("State", 6);
        //		}
        //		if (Input.GetKey (KeyCode.D)) 
        //		{
        //			transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
        //			anim.SetInteger ("State", -1);
        //		}
        //		if (Input.GetKey (KeyCode.A)) 
        //		{
        //			transform.position -= new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
        //			anim.SetInteger ("State", 4);
        //		}
    }
}
