using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private World world;
    public GameObject worldPrefab;
    private List<Event> registeredEvents = new List<Event>();

    private Dictionary<Event, List<Type>> eventHandlers = new Dictionary<Event, List<Type>>();

    private void Start() {

        worldPrefab = GameObject.Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);
        world = worldPrefab.GetComponent<World>();

        registeredEvents.Add(new Event());

        //string methodName = "Run";
        foreach(Event ev in registeredEvents) {
            List<Type> eventHandlerTypes = eventHandlers[ev];
            foreach(Type handlerType in eventHandlerTypes) {
                //Type type = ev.GetType();
                //if(type != null) {
                if(handlerType != null) { 
                    foreach(MethodInfo methodInfo in handlerType.GetMethods()) {
                        //MethodInfo methodInfo = type.GetMethod(methodName);

                        object[] attributes = methodInfo.GetCustomAttributes(false);

                        bool isEventListener = false;
                        int i = 0;
                        while(!isEventListener && i < attributes.Length) {
                            isEventListener = attributes[i].GetType() == typeof(EventListener);
                            i++;
                        }

                        if(isEventListener) {

                            ParameterInfo[] parameters = methodInfo.GetParameters();

                            bool isEventMethod = false;
                            if(parameters.Length == 1) {
                                ParameterInfo parameter = parameters[0];
                                Type parameterType = parameter.ParameterType;
                                Type eventType = typeof(Event);
                                isEventMethod = parameterType == eventType || parameterType.IsSubclassOf(eventType);
                            }

                            //object result = null;
                            //if(isEventMethod) {
                            //    object classInstance = Activator.CreateInstance(handlerType, null);
                            //    Event[] parametersArray = new Event[] { new Event() };
                            //    result = methodInfo.Invoke(classInstance, parametersArray);
                            //}
                            //Debug.Log(result);
                        }
                    }
                }
            }

        }
    }

    public void registerEvent(Event ev) {

        if(!registeredEvents.Contains(ev)) {
            registeredEvents.Add(ev);
        }

    }

}