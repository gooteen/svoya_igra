using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeItem : MonoBehaviour
{
    public int themeId;

    [SerializeField] private TMP_InputField _templateNameText;
    [SerializeField] private TMP_Text _templateText;

    [SerializeField] private Transform _container_questions;
    [SerializeField] private List<QuestionItem> _questionList;
    [SerializeField] private GameObject prefab_question;

    private string _themeName;

    public string ThemeName
    {
        get { return _themeName; }
        set
        {
            _themeName = value;
            if (_templateNameText != null)
            {
                _templateNameText.text = _themeName;
            }
            if(_templateText != null)
            {
                _templateText.text = _themeName;
            }
        }
    }

    public List<QuestionItem> QuestionList
    {
        get { return _questionList; }
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

    public void FillQuestionList(int chosenTemplate)
    {
        for(int i = 0; i < Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[themeId].questions.Count; i++)
        {
            GameObject questionObject = Instantiate(prefab_question, _container_questions);
            QuestionItem quesitonClass = questionObject.GetComponent<QuestionItem>();
            _questionList.Add(quesitonClass);
            quesitonClass.questionId = i;
            quesitonClass.themeId = themeId;
            quesitonClass.QuestionValue = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[themeId].questions[i].value.ToString();
        }
    }
}

