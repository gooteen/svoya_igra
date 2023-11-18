using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class QuestionEditor : MonoBehaviour
{
    [Header("Indices")]
    public int chosenTheme;
    public int chosenQuestion;

    public Dictionary<int, string> formatMap;

    [Header("UI")]
    public GameObject panel_questionEditor;
    public TMP_InputField input_value;
    public TMP_InputField input_question;
    public TMP_InputField input_answer;
    public TMP_InputField input_questionMedia;
    public TMP_InputField input_answerMedia;
    public TMP_Dropdown dropdown_questionType;
    public TMP_Dropdown dropdown_questionMediaType;
    public TMP_Dropdown dropdown_answerMediaType;
    public Image loadingStatusQuestion;
    public Image loadingStatusAnswer;

    public enum QuestionDataSavingMode { Answer, Value, Question };
    public enum MediaDataSavingMode { Answer, Question };
    public static QuestionEditor Instance
    {
        get;
        private set;
    }

    public void ConfigureQuestionEditor(int themeIndex, int questionIndex)
    {
        Debug.Log($"data: {themeIndex}, {questionIndex}");
        chosenTheme = themeIndex;
        chosenQuestion = questionIndex;
        FillData();
        panel_questionEditor.SetActive(true);
    }

    public void FillData()
    {
        loadingStatusQuestion.gameObject.SetActive(false);
        loadingStatusAnswer.gameObject.SetActive(false);

        input_value.text = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].value.ToString();
        input_question.text = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].questionText;
        input_answer.text = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].questionAnswer;

        string fullPath = Application.dataPath + Controller.Instance.Path;

        string question = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].mediaUrlQuestion;
        
        formatMap.TryGetValue(dropdown_questionMediaType.value, out string valueq);
        input_questionMedia.text = question;
        CheckIfFileExists(fullPath + "/" + input_questionMedia.text + valueq, MediaDataSavingMode.Question);

        string answer = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].mediaUrlAnswer;
        
        formatMap.TryGetValue(dropdown_answerMediaType.value, out string valuea);
        input_answerMedia.text = answer;
        CheckIfFileExists(fullPath + "/" + input_answerMedia.text + valuea, MediaDataSavingMode.Answer);
        
        dropdown_questionMediaType.value = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].mediaQuestionExtension;
        dropdown_answerMediaType.value = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].mediaAnswerExtension;
        dropdown_questionType.value = (int)Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].type;
    }

    public void UpdateQuestionType()
    {
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].type = (QuestionType)dropdown_questionType.value;
    }

    public void WriteValueData()
    {
        UpdateColumnValues(TemplateEditor.Instance.chosenTemplate);
    }

    public void WriteAnswerData()
    {
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].questionAnswer = input_answer.text;
    }

    public void WriteQuestionData()
    {
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
                    themes[chosenTheme].questions[chosenQuestion].questionText = input_question.text;
    }

    public void TrySaveQuestionMedia()
    {
        string fullPath = Application.dataPath + Controller.Instance.Path;
        
        formatMap.TryGetValue(dropdown_questionMediaType.value, out string value);
        CheckIfFileExists(fullPath + "/" + input_questionMedia.text + value, MediaDataSavingMode.Question);
        
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
            themes[chosenTheme].questions[chosenQuestion].mediaQuestionExtension = dropdown_questionMediaType.value;
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
            themes[chosenTheme].questions[chosenQuestion].mediaUrlQuestion = input_questionMedia.text;
        
        
    }

    public void TrySaveAnswerMedia()
    {
        string fullPath = Application.dataPath + Controller.Instance.Path;
       
        formatMap.TryGetValue(dropdown_answerMediaType.value, out string value);
        CheckIfFileExists(fullPath + "/" + input_answerMedia.text + value, MediaDataSavingMode.Answer);
        
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
            themes[chosenTheme].questions[chosenQuestion].mediaAnswerExtension = dropdown_answerMediaType.value;
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].
            themes[chosenTheme].questions[chosenQuestion].mediaUrlAnswer = input_answerMedia.text;
    }

    private void CheckIfFileExists(string path, MediaDataSavingMode mode)
    {
        Sprite targetSprite;
        bool cond = File.Exists(path);
        if (cond)
        {
            targetSprite = Resources.Load<Sprite>("Valid");
        } else
        {
            targetSprite = Resources.Load<Sprite>("Invalid");
        }

        if (mode == MediaDataSavingMode.Answer)
        {
            loadingStatusAnswer.gameObject.SetActive(true);
            loadingStatusAnswer.sprite = targetSprite;
        } else
        {
            loadingStatusQuestion.gameObject.SetActive(true);
            loadingStatusQuestion.sprite = targetSprite;
        }
    }

    private void UpdateColumnValues(int templateId)
    {
        foreach (Theme theme in Controller.Instance.GameData.data.userTemplates[templateId].themes)
        {
            theme.questions[chosenQuestion].value = int.Parse(input_value.text);
        }
        TemplateEditor.Instance.ConfigureEditorScreen(TemplateEditor.Instance.chosenTemplate);
    }

    private void Awake()
    {
        Instance = this;
        formatMap = new Dictionary<int, string>()
        {
            {0, ".png"},
            {1, ".mp3"},
            {2, ".mp4"}
        };
    }
}
