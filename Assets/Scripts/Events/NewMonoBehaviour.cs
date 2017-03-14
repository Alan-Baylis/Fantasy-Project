using System;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.All)]
public class HelpAttribute : System.Attribute {
    public readonly string Url;

    public string Topic   // Topic is a named parameter
    {
        get {
            return topic;
        }
        set {
            topic = value;
        }
    }

    public HelpAttribute(string url)   // url is a positional parameter
    {
        this.Url = url;
    }
    private string topic;
}

[HelpAttribute("Information on the class MyClass")]
class MyClass {
}
namespace AttributeAppl {
    class Program {
        public static void m(string[] args) {
            MemberInfo info = typeof(MyClass);
            object[] attributes = info.GetCustomAttributes(true);
            
            for(int i = 0; i < attributes.Length; i++) {
                //Console.WriteLine(attributes[i]);
                Debug.Log(attributes[i]);
            }

            //Console.ReadKey();
        }
    }
}