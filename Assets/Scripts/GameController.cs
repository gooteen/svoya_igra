using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Indices")]
    public int chosenTemplate;

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
    
    [SerializeField] private PlayersSO _playerData;

    public static GameController Instance
    {
        get;
        private set;
    }

    public PlayersSO Players
    {
        get { return _playerData; }
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
        } else
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
    
    private void Awake()
    {
        Instance = this;
    }
}
