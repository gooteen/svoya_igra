using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
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
    [SerializeField] private GameObject panel_alert_sameTitlesError;
    [SerializeField] private GameObject panel_intro;
    [SerializeField] private GameObject panel_main;

    [Header("Dynamic UI elements")]
    [SerializeField] private List<TemplateButton> _templateButtons;

    private VideoPlayer _videoPlayer;
    private AudioSource _audioPlayer;

    public static Controller Instance
    {
        get;
        private set;
    }

    public GameObject Panel_TemplateList
    {
        get { return panel_templateList; }
    }

    public AudioSource AudioPlayer
    {
        get { return _audioPlayer; }
    }

    public string Path
    {
        get { return _path; }
    }

    public GameDataSO GameData
    {
        get { return _gameData; }
    }

    public void Exit()
    {
        Application.Quit();
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
        if (DataNotCorrupted())
        {
            string path = Application.dataPath + _path;
            foreach (Template template in _gameData.data.userTemplates)
            {
                if (template.lastSavedTemplateName != template.templateName)
                {
                    File.Delete(path + "/" + template.lastSavedTemplateName + ".json.meta");
                    File.Delete(path + "/" + template.lastSavedTemplateName + ".json");
                    template.lastSavedTemplateName = template.templateName;
                }
                Utility.WriteFile(template, path + "/" + template.templateName + ".json");
            }
        } else
        {
            panel_alert_sameTitlesError.SetActive(true);
        }
    }

    public bool DataNotCorrupted()
    {
        for (int i = 0; i < _gameData.data.userTemplates.Count; i++)
        {
            for (int j = 0; j < _gameData.data.userTemplates.Count; j++)
            {
                if (j != i)
                {
                    if (_gameData.data.userTemplates[i].templateName == _gameData.data.userTemplates[j].templateName)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
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
                template.themes[i].questions[j].type = QuestionType.Regular;
            }
        }

        _gameData.data.userTemplates.Add(template);
    }

    public void DeleteTemplate(int index)
    {
        string path = Application.dataPath + _path;
        File.Delete(path + "/" + _gameData.data.userTemplates[index].lastSavedTemplateName + ".json.meta");
        File.Delete(path + "/" + _gameData.data.userTemplates[index].lastSavedTemplateName + ".json");
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
            FillTemplateButtonList(prefab_templateButton, _container_templates);
        }
        else
        {
            text_noTemplates.SetActive(true);
            panel_templateButtonView.SetActive(false);
        }
    }


    public void FillTemplateButtonList(GameObject prefab, Transform containter)
    {
        for (int i = 0; i < _gameData.data.userTemplates.Count; i++)
        {
            GameObject templateObject = Instantiate(prefab, containter);
            TemplateButton templateClass = templateObject.GetComponent<TemplateButton>();
            _templateButtons.Add(templateClass);
            templateClass.templateId = i;
            templateClass.TemplateName = _gameData.data.userTemplates[i].templateName;
        }
    }

    public void ClearTemplateButtonList()
    {
        foreach (TemplateButton button in _templateButtons)
        {
            Destroy(button.gameObject);
        }
        _templateButtons.Clear();
    }

    public void PlayThemesSound()
    {
        _audioPlayer.clip = Resources.Load<AudioClip>("themes");
        _audioPlayer.Play();
    }

    public void PlayCatSound()
    {
        _audioPlayer.clip = Resources.Load<AudioClip>("cat");
        _audioPlayer.Play();
    }

    public void PlayAuctionSound()
    {
        _audioPlayer.clip = Resources.Load<AudioClip>("auction");
        _audioPlayer.Play();
    }

    public void SkipIntro()
    {
        panel_intro.SetActive(false);
        panel_main.SetActive(true);
    }

    private void CreateNewGameDataSO()
    {
        Debug.Log("No data to load");
        UserData data = new UserData();
        _gameData.data = data;
    }

    private void Awake()
    {
        Instance = this;
        PullGameData();
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioPlayer = GetComponent<AudioSource>();
        _videoPlayer.loopPointReached += videoplayer => 
        {
            panel_intro.SetActive(false);
            panel_main.SetActive(true);
        };
    }
}
