using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public List<Template> userTemplates;
    public UserData()
    {
        userTemplates = new List<Template>();
    }
}

[System.Serializable]
public class Template
{
    public string templateName;
    public List<Theme> themes;
    public Template()
    {
        themes = new List<Theme>();
    }
}

[System.Serializable]
public class Theme
{
    public string name;
    public List<Question> questions;
    public Theme()
    {
        questions = new List<Question>();
    }
}

[System.Serializable]
public class Question
{
    public int value;
    public bool isCat;
    public string questionText;
    public string questionAnswer;
    public string mediaUrl;
}
