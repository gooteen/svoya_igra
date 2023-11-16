using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionEditor : MonoBehaviour
{
    [Header("Indices")]
    public int chosenTheme;
    public int chosenQuestion;

    //[Header("UI")]

    public static QuestionEditor Instance
    {
        get;
        private set;
    }

    public void ConfigureQuestionEditor(int themeIndex, int questionIndex)
    {
        chosenTheme = themeIndex;
        chosenQuestion = questionIndex;
    }

    private void Awake()
    {
        Instance = this;
    }
}
