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
    public int chosenTheme;
    public int chosenQuestion;

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

    private void Awake()
    {
        Instance = this;
    }
}
