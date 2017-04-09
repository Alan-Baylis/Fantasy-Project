//An event is a data type that can store multiple bits of information that is then passed to any event handling methods
//In order for an event to be cancellable, it must implement ICancellable
using System;
using UnityEngine;

public class PlayerMoveEvent : Event, ICancelable {

    private bool canceled;
    private Location location;
    private Vector3 movement;

    //The event can take any parameters you want in as arguments
    public PlayerMoveEvent(Location location, Vector3 movement) {
        this.canceled = false;
        this.location = location;
        this.movement = movement;
    }

    public Location getLocation() {
        return this.location;
    }

    public Vector3 getMovement() {
        return this.movement;
    }

    public bool isCanceled() {
        return this.canceled;
    }

    public void setCanceled(bool canceled) {
        this.canceled = canceled;
    }
}
