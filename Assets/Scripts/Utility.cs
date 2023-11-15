using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    public static void WriteFile(Template data, string path)
    {
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonString);
    }

    public static Template ReadFile(string path)
    {
        Debug.Log(path);
        StreamReader stream = new StreamReader(path);
        string content = stream.ReadToEnd();
        Debug.Log(content);
        Template data;
        try
        {
            data = JsonUtility.FromJson<Template>(content);
            if (data != null)
            {
                return data;
            }
            else
            {
                return null;
            }

        } catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }
    }
}
