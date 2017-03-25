//An event is a data type that can store multiple bits of information that is then passed to any event handling methods
//In order for an event to be cancellable, it must implement ICancellable
using System;

public class Event : ICancellable {

    //The event can take any paramters you want in as arguments
    public Event() {

    }

    //This method is required for the event to be cancellable
    public bool isCancelled() {
        throw new NotImplementedException();
    }
}
