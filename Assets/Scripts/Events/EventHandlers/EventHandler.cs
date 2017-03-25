using UnityEngine;

//Event handlers contain functions that are called whenever an event is triggered,
//All event handlers must implement IListener interface
public class EventHandler : IListener{

    //This is just an example event handler template it hows how the event listener method must be set out
    public EventHandler() {
    }

    //All methods that listen for events mut have the [EventListener] attribute
    [EventListener]
    //The method itself can be called whatever you want, but it can only take in one event class or any evetn subclass
    //as it's parameter
    public void onEventCall(Event ev) {
        //Do something...
    }

}
