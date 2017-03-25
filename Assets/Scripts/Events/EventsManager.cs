using System;
using System.Collections.Generic;
using System.Reflection;

//This class handles all event calling and event registration in the game using Reflection.
//If you don't know what Reflection is it is like accessing anything in an object, without being explicatley aware of the
//attributes name e.g: calling a method without knowing the methods name...
public class EventsManager {

    //Store a refernce to all the ReflectionData for event handlers that listen for a certain event call
    //The Type data type is the Type of the Event, and the List of ReflectionData is a list of all methods in 
    //event handlers tha listen for this event
    private Dictionary<Type, List<ReflectionData>> eventHandlers = new Dictionary<Type, List<ReflectionData>>();

    //Constructor takes in no arguments...
    public EventsManager() {

    }

    //This function calls all the methods in registered evetn handlers that listen for this event
    public void callEvent(Event ev) {
        //If the dictionary does not have any event handlers referenced to this event, then there is no point in calling the event
        if(!eventHandlers.ContainsKey(ev.GetType())) {
            return;
        }

        //Get a list of all the event handlers/method data types that are associated with this event
        List<ReflectionData> eventHandlerTypes = eventHandlers[ev.GetType()];
        //Iterate over all the ReflectionData objects in this list
        foreach(ReflectionData handlerData in eventHandlerTypes) {
            //Get the instance of the event handler that this ReflectionData stores
            IListener handlerInstance = handlerData.getEventHandler();
            //Get the method that listens for this event
            MethodInfo methodInfo = handlerData.getMethodInfo();
            //To call the event, parameters must be in the form of an array, the Event system only works if the 
            //listening method takes in only one parameter,
            //So create an array with only the event to call as the parameter
            Event[] parametersArray = new Event[] { ev };
            //Call the method on the refered instance and pass in the parameters
            methodInfo.Invoke(handlerInstance, parametersArray);
        }

    }

    //Save the inputed event handler's methods that take in an event as it's only parameter to the list of event listeners
    //for that event
    //Very Reflection Heavy...
    public void registerEventListener(IListener eventHandler) {

        //Get the type of the type of the eventHandler
        Type handlerType = eventHandler.GetType();
        //Iterate over all methods in the class
        foreach(MethodInfo methodInfo in handlerType.GetMethods()) {
            //Get any attributes associated with the method (attributes are the tags within the [] above the method definition)
            object[] attributes = methodInfo.GetCustomAttributes(false);

            //Initially we decide that most methods aren't event listeners, this covers us also if the method has no
            //attributes
            bool isEventListener = false;
            //Initialise the counting variable at 0
            int i = 0;
            //Iterate over all the attributes on the method as long as we haven't reached the last one or encountered the desired
            //attribute
            while(!isEventListener && i < attributes.Length) {
                //Compare the current attribute to the desired attribute, if they are the same type, then this
                //method has been tagged as an event listener
                isEventListener = attributes[i].GetType() == typeof(EventListener);
                //increment the indexing variable
                i++;
            }

            //If the method was tagged with the EventListener attribute
            if(isEventListener) {
                //Get all parameters that the method take in
                ParameterInfo[] parameters = methodInfo.GetParameters();
                //The method is assumed not to take in the correct amount and types of parameters initially
                bool isEventMethod = false;
                //The method can only be an event listener if it take in only 1 event as it's paramter
                if(parameters.Length == 1) {
                    //Get the Reflection data type that refers to the parameter of the method
                    ParameterInfo parameter = parameters[0];
                    //Get the type (the class) of the paramter
                    Type parameterType = parameter.ParameterType;
                    //The method is an event listener if it takes in an event as it's paramter, or any subclass of the 
                    //event class
                    Type eventType = typeof(Event);
                    isEventMethod = parameterType == eventType || parameterType.IsSubclassOf(eventType);
                }

                //If the method is an evetn listener, then we save a reference to it in the eventhandlers dictionary
                if(isEventMethod) {
                    //Get the type of the event parameter of the method
                    Type eventType = parameters[0].ParameterType;
                    //Initialise an empty list of Reflection Data that is associated with the event parameter
                    List<ReflectionData> handlerDataList = new List<ReflectionData>();
                    //If there already methods registered as event listeners for this event, then get the list of them from the dictionary
                    if(eventHandlers.ContainsKey(eventType)) {
                        handlerDataList = eventHandlers[eventType];
                    }
                    //Create a ReflectionData object to store a reference to the event handler instance and the event listener 
                    //method
                    ReflectionData handlerData = new ReflectionData(eventHandler, methodInfo);
                    //Add this data to the existing list of event handlers for the event
                    handlerDataList.Add(handlerData);
                    //Save this updated list to the dictionary with the calling event as it's reference
                    eventHandlers[eventType] = handlerDataList;
                }
            }
        }
    }

}
