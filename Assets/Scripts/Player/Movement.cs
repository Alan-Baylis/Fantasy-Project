using UnityEngine;

public class Movement : WorldObject {

    public float speed = 1f; //Sets the initial speed of the object. **Can be changed later.
    public Animator animator;
    private EventsManager eventsManager;
    public GameManager gameManager;
    public World world;

    void Start() {

        animator = GetComponent<Animator>();
        eventsManager = gameManager.getEventsManager();
        world = gameManager.getWorld();

    }

    // Update is called once per frame
    void Update() {

        if(eventsManager == null) {
            eventsManager = gameManager.getEventsManager();
        }

        //Get the key pressed and multiply it by the speed modifier, also smooth the movement with deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //The position is Vector3 so we ignore z by setting it to 0
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if(movement.magnitude == 0) {
            animator.SetInteger("State", 0);
            return;
        }
        if(world == null) {
            world = gameManager.getWorld();
        }

        Location location = new Location(world, transform.position);

        //Call the player move event and only continue with the code if the event isn't cancelled
        PlayerMoveEvent playerMoveEvent = new PlayerMoveEvent(location, movement);
        eventsManager.callEvent(playerMoveEvent);

        if(playerMoveEvent.isCanceled()) {
            return;
        }

        //Add it to the current position
        this.transform.position += movement;

        if(horizontal > 0) {
            animator.SetInteger("State", -1);
            if(horizontal == 0)
                animator.SetInteger("State", 5);
            // else
            // animator.SetInteger("State", -1);
        } else if(horizontal < 0) {
            animator.SetInteger("State", 4);
            if(horizontal == 0)
                animator.SetInteger("State", 1);
            //else
            // animator.SetInteger("State", 4);
        }
        if(vertical > 0) {
            animator.SetInteger("State", 2);
            if(vertical == 0)
                animator.SetInteger("State", 0);
            // else
            // animator.SetInteger("State", 2);
        } else if(vertical < 0) {
            animator.SetInteger("State", 6);
            if(vertical == 0)
                animator.SetInteger("State", 3);
            //else
            // animator.SetInteger("State", 6);
        }
        //if(horizontal == 0 && vertical == 0) {
        //    animator.SetInteger("State", 0);
        //}



















        //See the difference?
        //        //if(Input.GetAxis("Horizontal")) This can be used instead of GetKey, as it allows you to change the keys in Unity.
        //		if (Input.GetKey (KeyCode.W)) 
        //		{    
        //			transform.position += new Vector3 (0.0f, speed * Time.deltaTime, 0.0f);
        //			animator.SetInteger ("State", 2);
        //		}
        //		if (Input.GetKey (KeyCode.S)) 
        //		{  
        //			transform.position -= new Vector3 (0.0f, speed * Time.deltaTime, 0.0f);
        //			animator.SetInteger ("State", 6);
        //		}
        //		if (Input.GetKey (KeyCode.D)) 
        //		{
        //			transform.position += new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
        //			animator.SetInteger ("State", -1);
        //		}
        //		if (Input.GetKey (KeyCode.A)) 
        //		{
        //			transform.position -= new Vector3 (speed * Time.deltaTime, 0.0f, 0.0f);
        //			animator.SetInteger ("State", 4);
        //		}
    }
}
