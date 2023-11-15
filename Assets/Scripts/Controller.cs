﻿using System.Collections;
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
    [SerializeField] private GameObject panel_templateList;

    [Header("Dynamic UI elements")]
    [SerializeField] private List<TemplateButton> _templateButtons;

    public static Controller Instance
    {
        get;
        private set;
    }

    public GameObject Panel_TemplateList
    {
        get { return panel_templateList; }
    }

    public GameDataSO GameData
    {
        get { return _gameData; }
    }

    public void PullGameData()
    {
        string path = Application.dataPath + _path;

        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string[] files = Directory.GetFiles(path);

        if (files.Length > 0)
        {
            List<Template> templateList = new List<Template>();
            foreach (string file in files)
            {
                Template template = Utility.ReadFile(file);
                if (template != null)
                {
                    templateList.Add(template);
                }
                else
                {
                    Debug.Log($"{file} is not compatible, skipping...");
                }
            }
            _gameData.data.userTemplates = templateList;
        } else
        {
            CreateNewGameDataSO();
        }
    }

    public void SaveGameData()
    {
        string path = Application.dataPath + _path;
        foreach (Template template in _gameData.data.userTemplates)
        {
            Utility.WriteFile(template, path + "/" + template.templateName + ".json");
        }
    }

    public void AddNewTemplate()
    {
        Template template = new Template();

        template.templateName = $"Новый шаблон раунда {_gameData.data.userTemplates.Count + 1}";
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

    public void DeleteTemplate(int index)
    {
        string path = Application.dataPath + _path;
        File.Delete(path + "/" + _gameData.data.userTemplates[index].templateName + ".meta");
        File.Delete(path + "/" + _gameData.data.userTemplates[index].templateName + ".json");
        _gameData.data.userTemplates.RemoveAt(index);
        ConfigureTemplateList();
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

    private void CreateNewGameDataSO()
    {
        Debug.Log("No data to load");
        UserData data = new UserData();
        _gameData.data = data;
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
