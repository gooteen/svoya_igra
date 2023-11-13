using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Controller : MonoBehaviour
{
    [SerializeField] private string _path;
    [SerializeField] private GameDataSO _gameData;

    [Header("UI")]
    [SerializeField] private Transform _container_templates;
    [SerializeField] private GameObject text_noTemplates;
    [SerializeField] private GameObject prefab_templateButton;
    [SerializeField] private GameObject panel_templateButtonView;

    [Header("Dynamic UI elements")]
    [SerializeField] private List<TemplateButton> _templateButtons;

    public static Controller Instance
    {
        get;
        private set;
    }

    public GameDataSO GameData
    {
        get { return _gameData; }
    }

    public void PullGameData()
    {
        string path = Application.dataPath + _path;
        if (File.Exists(path))
        {
            _gameData.data = Utility.ReadFile(path);
        } else
        {
            UserData data = new UserData();
            Utility.WriteFile(data, path);
            _gameData.data = Utility.ReadFile(path);
        }
    }

    public void SaveGameData()
    {
        string path = Application.dataPath + _path;
        Utility.WriteFile(_gameData.data, path);
    }

    public void AddNewTemplate()
    {
        Template template = new Template();

        template.templateName = "Новый шаблон раунда";
        template.themes = new List<Theme>();

        for (int i = 0; i < 5; i++)
        {
            template.themes.Add(new Theme());
            template.themes[i].name = "Новая тема";
            template.themes[i].questions = new List<Question>();
            for (int j = 0; j < 5; j++)
            {
                template.themes[i].questions.Add(new Question());
                template.themes[i].questions[j].value = (j+1) * 100;
            }
        }

        _gameData.data.userTemplates.Add(template);
    }

    public void ConfigureTemplateList()
    {
        ClearTemplateButtonList();
        if (_gameData.data.userTemplates.Count != 0)
        {
            text_noTemplates.SetActive(false);
            panel_templateButtonView.SetActive(true);
            FillTemplateButtonList();
        }
        else
        {
            text_noTemplates.SetActive(true);
            panel_templateButtonView.SetActive(false);
        }
    }

    private void FillTemplateButtonList()
    {
        for (int i = 0; i < _gameData.data.userTemplates.Count; i++)
        {
            GameObject templateObject = Instantiate(prefab_templateButton, _container_templates);
            TemplateButton templateClass = templateObject.GetComponent<TemplateButton>();
            _templateButtons.Add(templateClass);
            templateClass.templateId = i;
            templateClass.TemplateName = _gameData.data.userTemplates[i].templateName;
        }
    }

    private void ClearTemplateButtonList()
    {
        foreach (TemplateButton button in _templateButtons)
        {
            Destroy(button.gameObject);
        }
        _templateButtons.Clear();
    }

    private void Awake()
    {
        Instance = this;
        PullGameData();
        ConfigureTemplateList();
    }
}
