
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public class EventHandler {

    ////Reflexive method calling:
    //public string[] foo(string[] parameters) {
    //    //do something...
    //    return parameters;
    //}

    //public static void reflexiveCall(string methodName) {

    //    Assembly assembly = Assembly.LoadFile("...EventHandler.cs");
    //    Type type = assembly.GetType("EventHandler");
    //    if(type != null) {
    //        MethodInfo methodInfo = type.GetMethod(methodName);

    //        if(methodInfo != null) {
    //            object result = null;
    //            ParameterInfo[] parameters = methodInfo.GetParameters();
    //            object classInstance = Activator.CreateInstance(type, null);

    //            if(parameters.Length == 0) {
    //                result = methodInfo.Invoke(classInstance, null);
    //            } else {
    //                object[] parametrsArray = new object[] { "Hello" };
    //                result = methodInfo.Invoke(classInstance, parametrsArray);
    //            }
    //            Debug.Log(result);
    //        }

    //    }

    //}

}

//namespace TestAssembly {

//    public class Main {

//        public void Run(string parameters) {
//            //do something
//            Debug.Log(parameters);
//        }

//        public void TestNoParameters() {
//            Debug.Log("Hi");
//            //Do something
//        }

//    }

//}

//public class TestReflection {

//    public void Test(string methodName) {

//        Assembly assembly = Assembly.LoadFile("C:/Users/TiernanS/Dropbox/2-D Fantasy Project/Assets/Scripts/Events/Assembly.dll");
//        Type type = assembly.GetType("TestAssembly.Main");

//        if(type != null) {

//            MethodInfo methodInfo = type.GetMethod(methodName);

//            if(methodInfo != null) {
//                object result = null;
//                ParameterInfo[] parameters = methodInfo.GetParameters();

//                object classInstance = Activator.CreateInstance(type, null);

//                if(parameters.Length == 0) {
//                    result = methodInfo.Invoke(classInstance, null);
//                } else {
//                    object[] parametersArray = new object[] { "Hello" };
//                    result = methodInfo.Invoke(classInstance, parametersArray);
//                }

//            }

//        }

//    }

//}
