using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeItem : MonoBehaviour
{
    public int themeId;

    [SerializeField] private TMP_InputField _templateNameText;

    [SerializeField] private Transform _container_questions;
    [SerializeField] private List<QuestionItem> questionList;
    [SerializeField] private GameObject prefab_question;

    private string _themeName;

    public string ThemeName
    {
        get { return _themeName; }
        set
        {
            _themeName = value;
            _templateNameText.text = _themeName;
        }
    }

    private void Awake()
    {
        FillQuestionList();
    }

    public void DeleteTheme()
    {
        TemplateEditor.Instance.ClearThemeRow(themeId);
    }
    
    public void UpdateThemeName()
    {
        _themeName = _templateNameText.text;
        Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].themes[themeId].name = _themeName;
    }

    private void FillQuestionList()
    {
        for(int i = 0; i < Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].themes[themeId].questions.Count; i++)
        {
            GameObject questionObject = Instantiate(prefab_question, _container_questions);
            QuestionItem quesitonClass = questionObject.GetComponent<QuestionItem>();
            questionList.Add(quesitonClass);
            quesitonClass.questionId = i;
            quesitonClass.themeId = themeId;
            quesitonClass.QuestionValue = Controller.Instance.GameData.data.userTemplates[TemplateEditor.Instance.chosenTemplate].themes[themeId].questions[i].value.ToString();
        }
    }
}

