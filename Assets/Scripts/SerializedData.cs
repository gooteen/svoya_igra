using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public Template[] userTemplates;
}

[System.Serializable]
public class Template
{
    public string id;
    public string templateName;
    public Round[] rounds;
}

[System.Serializable]
public class Round
{
    public Theme[] themes;
}

[System.Serializable]
public class Theme
{
    public string id;
    public string name;
    public Question[] questions;
}

[System.Serializable]
public class Question
{
    public string id;
    public int value;
    public bool isCat;
    public string questionText;
    public string questionAnswer;
    public string mediaUrl;
}
