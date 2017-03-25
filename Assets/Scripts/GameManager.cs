using UnityEngine;

//This is the core object that handles all game mechanics in a world
public class GameManager : MonoBehaviour {

    //Save a reference to the world the GameManager is handling
    private static World world;
    //The prefab GameObject for all world objects
    public GameObject worldPrefab;

    //The event manager for the game
    private static EventsManager eventsManager;

    //On start up, generate the world and create the events manager
    private void Start() {

        worldPrefab = GameObject.Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);
        world = worldPrefab.GetComponent<World>();

        eventsManager = new EventsManager();
        //An example of how you would register an event handler to be a listener
        eventsManager.registerEventListener(new EventHandler());
        //An example of how you would call an event
        eventsManager.callEvent(new Event());

    }

    //Get the world object that has been generated
    public static World getWorld() {
        return world;
    }

    //Return the reference to the events manager
    public static EventsManager getEventsManager() {
        return eventsManager;
    }

}