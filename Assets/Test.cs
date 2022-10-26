using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var a = new A<int>();
        var b = new B();
        var c = new C();
        Debug.Log($", {B.tValue},  {C.tValue}");
        b.Test();
        Debug.Log($", {B.tValue}, {C.tValue}");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class A<T>
{
    public static T tValue;
    public static string value = "A";
}
public class B: A<string>
{
    public B()
    {
        tValue = "B";
    }
    public void Test()
    {
        value = "B";
        tValue = "BB";
    }

}

public class C : A<string>
{
    public C()
    {
        tValue = "C";
    }
    public void Test()
    {
        value = "C";
    }

}


