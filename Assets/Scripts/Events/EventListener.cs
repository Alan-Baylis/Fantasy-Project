using System;

//This class represent the attribute tag that must be applied to all event listener methods
//in order for it to be called by the event manager

//This means that the attribute can only be applied to methods
[AttributeUsage(AttributeTargets.Method)]
public class EventListener : Attribute {
    //The constructor takes no arguments.
    public EventListener() {

    }

}

