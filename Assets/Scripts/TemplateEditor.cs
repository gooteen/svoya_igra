using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TemplateEditor : MonoBehaviour
{
    [Header("Indices")]
    public int chosenTemplate;

    [Header("UI")]
    [SerializeField] private Transform _container_themes;
    [SerializeField] private GameObject prefab_themeItem;
    [SerializeField] private GameObject _panel_editor;
    [SerializeField] private TMP_InputField _input_title;

    [Header("Dynamic UI elements")]
    [SerializeField] private List<ThemeItem> _themeButtons;

    public GameObject Panel_Editor
    {
        get { return _panel_editor; }
    }
    public static TemplateEditor Instance
    {
        get;
        private set;
    }

    public void ConfigureEditorScreen(int templateIndex)
    {
        chosenTemplate = templateIndex;
        ClearThemesList();
        FillThemesList();
        SetTemplateTitleInputField();
        _panel_editor.SetActive(true);
        Controller.Instance.Panel_TemplateList.SetActive(false);
    }

    public void SaveThemeTitle()
    {
        Controller.Instance.GameData.data.userTemplates[chosenTemplate].templateName = _input_title.text;
    }

    public void SetTemplateTitleInputField()
    {
        _input_title.text = Controller.Instance.GameData.data.userTemplates[chosenTemplate].templateName;
    }

    public void FillThemesList()
    {
        for (int i = 0; i < Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes.Count; i++)
        {
            GameObject themeObject = Instantiate(prefab_themeItem, _container_themes);
            ThemeItem themeClass = themeObject.GetComponent<ThemeItem>();
            _themeButtons.Add(themeClass);
            themeClass.themeId = i;
            themeClass.ThemeName = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[i].name;
            themeClass.FillQuestionList(chosenTemplate);
        }
    }

    public void ClearThemesList()
    {
        foreach (ThemeItem button in _themeButtons)
        {
            Destroy(button.gameObject);
        }
        _themeButtons.Clear();
    }

    public void ClearQuestionColumn(int id)
    {
        foreach (Theme theme in Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes)
        {
            theme.questions.RemoveAt(id);
        }
        ClearThemesList();
        FillThemesList();
    }

    public void AddQuestionColumn()
    {
        foreach (Theme theme in Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes)
        {
            Question question = new Question();
            question.value = 100;
            theme.questions.Add(question);
        }
        ClearThemesList();
        FillThemesList();
    }

    public void ClearThemeRow(int id)
    {
        Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes.RemoveAt(id);
        ClearThemesList();
        FillThemesList();
    }

    public void AddThemeRow()
    {
        int questionsNum = 0;

        if (Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes.Count == 0)
        {
            questionsNum = 5;
        } else
        {
            questionsNum = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[0].questions.Count;
        }

        Theme theme = new Theme();
        theme.name = "Новая тема";
        for (int i = 0; i < questionsNum; i++)
        {
            theme.questions.Add(new Question());
            theme.questions[i].value = (i + 1) * 100;
        }
        Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes.Add(theme);
        ClearThemesList();
        FillThemesList();
    }

    private void Awake()
    {
        Instance = this;
    }
}
