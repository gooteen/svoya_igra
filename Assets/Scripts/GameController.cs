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

    [Header("UI")]
    [SerializeField] GameObject _prefab_TemplateItem;
    [SerializeField] Transform _container_TemplateItems;
    [SerializeField] GameObject _prefab_PlayerItem;

    [SerializeField] GameObject _text_noTemplates;
    [SerializeField] GameObject _text_noPlayers;

    [SerializeField] Transform _container_PlayerItems;

    [SerializeField] List<PlayerItem> _list_PlayerItems;

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

    public void ClearPlayerList()
    {
        foreach (PlayerItem item in _list_PlayerItems)
        {
            Destroy(item.gameObject);
        }
        _list_PlayerItems.Clear();
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
            playerClass.text_score.text = _playerData.players[i].score.ToString();
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
