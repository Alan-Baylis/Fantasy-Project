//An event is a data type that can store multiple bits of information that is then passed to any event handling methods
//In order for an event to be cancellable, it must implement ICancellable
using System;

public class EntityEvent : Event {

    //The event can take any paramters you want in as arguments
    public EntityEvent() {

    }

}
