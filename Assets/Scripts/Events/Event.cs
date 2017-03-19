
using UnityEngine;

public class Event {

    public Event() {

    }

    public void TEST() {

    }

    [EventListener]
    public string Run(Event ev) {
        return "HI";
    }

}
