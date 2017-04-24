using System.Reflection;

//THis class is used simply to have an easy storage of the data necessary to make a
//reflexive method call on an eventhandler when an event is called
public class ReflectionData {

    //The reference to the instacne of the event handler, all event handlers must iplement the Listener interface
    private IListener eventHandler;
    //A reflection data type that represents the method to which the event is called
    private MethodInfo methodInfo;

    //The constructor saves the inputed instance and method reference.
    public ReflectionData(IListener eventHandler, MethodInfo methodInfo) {

        this.eventHandler = eventHandler;
        this.methodInfo = methodInfo;

    }

    //Get the instance of the event handler to which this ReflectionData object refers
    public IListener getEventHandler() {
        return this.eventHandler;
    }

    //Get the method within the instance that takes an event in as a parameter
    public MethodInfo getMethodInfo() {
        return this.methodInfo;
    }

}
