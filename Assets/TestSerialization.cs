using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestSerialization
{
    public string one;
    public TestSerialization1 test1;

    public TestSerialization()
    {
        one = "strug";
        test1 = new TestSerialization1();
    }

}

[System.Serializable]
public class TestSerialization1
{
    public TestSerialization2[] test2s;
    public TestSerialization1()
    {
        test2s = new TestSerialization2[2];
        for (int i = 0; i < test2s.Length; i++)
        {
            test2s[i] = new TestSerialization2();
        }
    }
}

[System.Serializable]
public class TestSerialization2
{
    public string punk;
    public string punk2;
    public TestSerialization3 test3;

    public TestSerialization2()
    {
        punk = "string";
        punk2 = "string2";
        test3 = new TestSerialization3();
    }
}

[System.Serializable]
public class TestSerialization3
{
    public string test3;
    public TestSerialization3()
    {
        test3 = "dsd";
    }
}
