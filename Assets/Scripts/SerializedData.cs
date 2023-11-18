using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionType { Regular, Auction, Cat }

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
    public string lastSavedTemplateName;
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
    public QuestionType type;
    public string questionText;
    public string questionAnswer;
    public string mediaUrlQuestion;
    public int mediaQuestionExtension;
    public string mediaUrlAnswer;
    public int mediaAnswerExtension;
}
