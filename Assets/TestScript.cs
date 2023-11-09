using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text text;

    void Start()
    {
        ReadString();
    }

    void Update()
    {
        
    }

    public void ReadString()
    {
        string path = Application.dataPath + "/test.txt";
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string var = reader.ReadToEnd();
            Debug.Log(var);
            text.text = var;
            reader.Close();
        } else
        {
            Debug.Log("Fuck");
        }

    }
}
