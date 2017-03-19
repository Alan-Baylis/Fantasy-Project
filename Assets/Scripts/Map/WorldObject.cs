using UnityEngine;

public class WorldObject : MonoBehaviour {

    private Location location;

    public void initialise(Location location) {

        this.location = location;

    }

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
