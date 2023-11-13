using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    public static void WriteFile(UserData data, string path)
    {
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonString);
    }

    public static UserData ReadFile(string path)
    {
        StreamReader stream = new StreamReader(path);
        string content = stream.ReadToEnd();
        UserData data = new UserData();
        data = JsonUtility.FromJson<UserData>(content);
        return data;
    }
}
