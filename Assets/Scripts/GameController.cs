using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayersSO _playerData;

    [Header("Indices")]
    public int chosenTemplate;
    public int chosenTheme;
    public int chosenQuestion;

    [Header("UI_Lobby")]
    [SerializeField] private GameObject _prefab_TemplateItem;
    [SerializeField] private Transform _container_TemplateItems;
    [SerializeField] private GameObject _prefab_PlayerItem;
    [SerializeField] private GameObject _text_noTemplates;
    [SerializeField] private GameObject _text_noPlayers;
    [SerializeField] private Transform _container_PlayerItems;
    [SerializeField] private List<PlayerItem> _list_PlayerItems;
    [SerializeField] private GameObject _panel_lobby;

    [Header("UI_Game")]
    [SerializeField] private TMP_Text _template_name;
    [SerializeField] private Transform _container_themes;
    [SerializeField] private GameObject _prefab_ThemeItem;
    [SerializeField] private List<ThemeItem> _themeButtons;
    [SerializeField] private Transform _container_PlayerItemsGame;
    [SerializeField] private GameObject _prefab_PlayerItemGame;
    [SerializeField] private GameObject _panel_Alert;
    [SerializeField] private GameObject _panel_gameScreen;
    [SerializeField] private GameObject _panel_questionScreen;
    
    [Header("Media Management")]
    [SerializeField] private Sprite _mediaPause;
    [SerializeField] private Sprite _mediaPlay;
    [SerializeField] private Button _videoControlButton;
    [SerializeField] private Button _audioControlButton;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private AudioSource _audioPlayer;
    [SerializeField] private string _mediaName;
    [SerializeField] private string _mediaExtension;

    [Header("Plain Text Layout")]
    [SerializeField] private GameObject _panel_plainText;
    [SerializeField] private TMP_Text _text_questionBodyPlain;
    [SerializeField] private TMP_Text _text_isAuctionPlain;
    [SerializeField] private TMP_Text _text_isCatPlain;
    [SerializeField] private Button _button_forward_plain;
    [SerializeField] private Button _button_back_plain;

    [Header("Media Layout")]
    [SerializeField] private GameObject _panel_media;
    [SerializeField] private GameObject _placeholder_mp4;
    [SerializeField] private GameObject _placeholder_mp3;
    [SerializeField] private Image _image_mediaPng;
    [SerializeField] private TMP_Text _text_questionBodyMedia;
    [SerializeField] private TMP_Text _text_isAuctionMedia;
    [SerializeField] private TMP_Text _text_isCatMedia;
    [SerializeField] private Button _button_forward_media;
    [SerializeField] private Button _button_back_media;

    [Header("Score Calculation")]
    [SerializeField] private GameObject _panel_calculation;
    [SerializeField] private List<CalculationItem> _calculationItems;
    [SerializeField] private Transform _container_calcItems;
    [SerializeField] private GameObject _prefab_calculationItem;

    public static GameController Instance
    {
        get;
        private set;
    }

    public PlayersSO Players
    {
        get { return _playerData; }
    }

    public GameObject PanelGameScreen
    {
        get { return _panel_gameScreen; }
    }

    public GameObject PanelQuestionScreen
    {
        get { return _panel_questionScreen; }
    }

    public void ConfigureLobbyScreen()
    {
        chosenTemplate = -1;
        Controller.Instance.ClearTemplateButtonList();
        Controller.Instance.FillTemplateButtonList(_prefab_TemplateItem, _container_TemplateItems);
        if (Controller.Instance.GameData.data.userTemplates.Count == 0)
        {
            _text_noTemplates.SetActive(true);
        } else
        {
            _text_noTemplates.SetActive(false);
        }
        ClearPlayerList();
        FillPlayerList();
    }

    public void SetIndices(int themeId, int questionId)
    {
        chosenTheme = themeId;
        chosenQuestion = questionId;
    }

    public void ConfigureQuestionWindow(int mode)
    {
        Controller.Instance.AudioPlayer.Stop();
        _mediaName = "";
        _mediaExtension = "";
        _panel_plainText.SetActive(false);
        _panel_media.SetActive(false);
        _panel_calculation.SetActive(false);
        SetWindowLayout(mode);
    }

    public void ConfigureCalculationWindow()
    {
        Debug.Log("worj");
        _panel_plainText.SetActive(false);
        _panel_media.SetActive(false);
        ClearCalcItemsList();
        FillCalcItemsList();
        _panel_calculation.SetActive(true);
    }

    public void CalculateScore()
    {
        foreach (CalculationItem item in _calculationItems)
        {
            int parsedValue = 0;

            try
            {
                parsedValue = int.Parse(item.inputField.text);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            if (item.dropdown.value == 1)
            {
                _playerData.players[item.id].score -= parsedValue;
                if (_playerData.players[item.id].score < 0)
                {
                    _playerData.players[item.id].score = 0;
                }
            } else if (item.dropdown.value == 2)
            {
                _playerData.players[item.id].score += parsedValue;
            }
        }
    }

    public void SetMarkerToQuestion()
    {
        _themeButtons[chosenTheme].QuestionList[chosenQuestion].ActivateMarker();
    }

    public void FillCalcItemsList()
    {
        for (int i = 0; i < _playerData.players.Count; i++)
        {
            GameObject calculationObject = Instantiate(_prefab_calculationItem, _container_calcItems);
            CalculationItem calculationClass = calculationObject.GetComponent<CalculationItem>();
            _calculationItems.Add(calculationClass);
            calculationClass.id = i;
            calculationClass.dropdown.value = 0;
            calculationClass.inputField.text = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].value.ToString();
            calculationClass.playerName.text = _playerData.players[i].name;
        }
    }

    public void ClearCalcItemsList()
    {
        foreach (CalculationItem item in _calculationItems)
        {
            Destroy(item.gameObject);
        }
        _calculationItems.Clear();
    }

    public void SetWindowLayout(int mode)
    {
        _text_isCatMedia.gameObject.SetActive(false);
        _text_isCatPlain.gameObject.SetActive(false);
        _text_isAuctionMedia.gameObject.SetActive(false);
        _text_isAuctionPlain.gameObject.SetActive(false);

        string windowText = "";

        bool isCat = false;
        bool isAuction = false;

        if (mode == 0)
        {
             _mediaName = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].mediaUrlQuestion;
            QuestionEditor.Instance.formatMap.TryGetValue(Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].mediaQuestionExtension, out _mediaExtension);
            windowText = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].questionText;
            SetQuestionButtons();
            if (Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].type == QuestionType.Auction)
            {
                isAuction = true;
            } else if (Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].type == QuestionType.Cat)
            {
                isCat = true;
            }

        } else
        {
            _mediaName = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].mediaUrlAnswer;
            QuestionEditor.Instance.formatMap.TryGetValue(Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].mediaAnswerExtension, out _mediaExtension);
            windowText = Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes[chosenTheme].questions[chosenQuestion].questionAnswer;
            SetAnswerButtons();
        }

        string path = Application.dataPath + Controller.Instance.Path + "/" + _mediaName + _mediaExtension;
        if (File.Exists(path))
        {
            SetMediaContent(path);
            _text_questionBodyMedia.text = windowText;
            _panel_media.SetActive(true);

            if (isCat)
            {
                _text_isCatMedia.gameObject.SetActive(true);
                Controller.Instance.PlayCatSound();
            }
            else if (isAuction)
            {
                _text_isAuctionMedia.gameObject.SetActive(true);
                Controller.Instance.PlayAuctionSound();
            }
        } else
        {
            _text_questionBodyPlain.text = windowText;
            _panel_plainText.SetActive(true);

            if (isCat)
            {
                _text_isCatPlain.gameObject.SetActive(true);
                Controller.Instance.PlayCatSound();
            }
            else if (isAuction)
            {
                _text_isAuctionPlain.gameObject.SetActive(true);
                Controller.Instance.PlayAuctionSound();
            }
        }
    }

    public void SetMediaContent(string path)
    {
        _placeholder_mp4.SetActive(false);
        _placeholder_mp3.SetActive(false);
        _image_mediaPng.gameObject.SetActive(false);

        if (_mediaExtension == ".png")
        {
            StartCoroutine(DownloadImage(path));
        }
        else if (_mediaExtension == ".mp3")
        {
            _audioControlButton.image.sprite = _mediaPlay;
            StartCoroutine(DownloadAudio(path));
        }
        else if (_mediaExtension == ".mp4")
        {
            _videoControlButton.image.sprite = _mediaPlay;
            LoadVideo(path);
        }
    }

    public void LoadVideo(string url)
    {
        _videoPlayer.url = url;
        _placeholder_mp4.SetActive(true);
    }

    public void ToggleVideoState()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();
            _videoControlButton.image.sprite = _mediaPlay;
        } else
        {
            _videoPlayer.Play();
            _videoControlButton.image.sprite = _mediaPause;
        }
    }

    public void ToggleAudioState()
    {
        if (_audioPlayer.isPlaying)
        {
            _audioPlayer.Pause();
            _audioControlButton.image.sprite = _mediaPlay;
        }
        else
        {
            _audioPlayer.Play();
            _audioControlButton.image.sprite = _mediaPause;
        }
    }

    public void ConfigureGameScreen()
    {
        if (chosenTemplate != -1 && _playerData.players.Count != 0)
        {
            ClearThemesList();
            FillThemesList();
            ClearPlayerList();
            FillPlayerListInGame();
            _template_name.text = Controller.Instance.GameData.data.userTemplates[chosenTemplate].templateName;
            _panel_gameScreen.SetActive(true);
            _panel_lobby.SetActive(false);
            Controller.Instance.PlayThemesSound();
        }
        else
        {
            _panel_Alert.SetActive(true);
        }
    }

    public void FillThemesList()
    {
        for (int i = 0; i < Controller.Instance.GameData.data.userTemplates[chosenTemplate].themes.Count; i++)
        {
            GameObject themeObject = Instantiate(_prefab_ThemeItem, _container_themes);
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

    public void ClearPlayerList()
    {
        foreach (PlayerItem item in _list_PlayerItems)
        {
            Destroy(item.gameObject);
        }
        _list_PlayerItems.Clear();
    }

    public void FillPlayerListInGame()
    {
        for (int i = 0; i < _playerData.players.Count; i++)
        {
            GameObject playerObject = Instantiate(_prefab_PlayerItemGame, _container_PlayerItemsGame);
            PlayerItem playerClass = playerObject.GetComponent<PlayerItem>();
            _list_PlayerItems.Add(playerClass);
            playerClass.index = i;
            playerClass.text_name.text = _playerData.players[i].name;
            playerClass.text_score.text = _playerData.players[i].score.ToString();
        }
    }

    public void AddNewPlayer()
    {
        PlayerData data = new PlayerData(0, "Новый игрок");
        _playerData.players.Add(data);
        ClearPlayerList();
        FillPlayerList();
    }

    public void FillPlayerList()
    {
        for (int i = 0; i < _playerData.players.Count; i++)
        {
            GameObject playerObject = Instantiate(_prefab_PlayerItem, _container_PlayerItems);
            PlayerItem playerClass = playerObject.GetComponent<PlayerItem>();
            _list_PlayerItems.Add(playerClass);
            playerClass.index = i;
            playerClass.input_name.text = _playerData.players[i].name;
            playerClass.input_score.text = _playerData.players[i].score.ToString();
        }
        if(_playerData.players.Count == 0)
        {
            _text_noPlayers.SetActive(true);
        } else
        {
            _text_noPlayers.SetActive(false);
        }
    }

    private void SetQuestionButtons()
    {
        _button_back_media.onClick.RemoveAllListeners();
        _button_back_media.onClick.AddListener(
            delegate
            {
                _panel_questionScreen.SetActive(false);
                _panel_gameScreen.SetActive(true);
            });

        _button_back_plain.onClick.RemoveAllListeners();
        _button_back_plain.onClick.AddListener(
            delegate
            {
                _panel_questionScreen.SetActive(false);
                _panel_gameScreen.SetActive(true);
            });

        _button_forward_media.onClick.RemoveAllListeners();
        _button_forward_media.onClick.AddListener(
            delegate
            {
                ConfigureQuestionWindow(1);
            });

        _button_forward_plain.onClick.RemoveAllListeners();
        _button_forward_plain.onClick.AddListener(
            delegate
            {
                ConfigureQuestionWindow(1);
            });
    }

    private void SetAnswerButtons()
    {
        _button_back_media.onClick.RemoveAllListeners();
        _button_back_media.onClick.AddListener(
            delegate
            {
                ConfigureQuestionWindow(0);

            });

        _button_back_plain.onClick.RemoveAllListeners();
        _button_back_plain.onClick.AddListener(
            delegate
            {
                ConfigureQuestionWindow(0);
            });

        _button_forward_media.onClick.RemoveAllListeners();
        _button_forward_media.onClick.AddListener(ConfigureCalculationWindow);

        _button_forward_plain.onClick.RemoveAllListeners();
        _button_forward_plain.onClick.AddListener(ConfigureCalculationWindow);
    }

    private IEnumerator DownloadAudio(string url)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            _audioPlayer.clip = clip;
            _placeholder_mp3.SetActive(true);
        }
    }
    private IEnumerator DownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            _image_mediaPng.overrideSprite = sprite;
            _image_mediaPng.gameObject.SetActive(true);
        }
    }

    private void Awake()
    {
        Instance = this;
        _videoPlayer.loopPointReached += v => { _videoControlButton.image.sprite = _mediaPlay; };
    }
}
