using UnityEngine;

//This class is the super class of all things that can be created in the game as an object in the world
public class WorldObject : MonoBehaviour {

    //All objects have a location intrinsically tied to them
    private Location location;

    //Apply the location to the GameObject and save it once loaded
    public void initialise(Location location) {

        this.location = location;
        transform.position = location.asTransform();

    }

    //Getters for various aspects of the WorldObject
    public virtual World getWorld() {
        return getLocation().getWorld();
    }

    public virtual Location getLocation() {
        return this.location;
    }

    public virtual void setLocation(Location location) {
        this.location = location;
    }


}
