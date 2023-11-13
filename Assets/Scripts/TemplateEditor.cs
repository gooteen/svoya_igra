using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TemplateEditor : MonoBehaviour
{
    [Header("Indices")]
    public int _chosenTemplate;
    public int _chosenTheme;
    public int _chosenQuestion;

    [Header("UI")]
    [SerializeField] private Transform _container_themes;
    [SerializeField] private GameObject prefab_themeItem;

    [Header("Dynamic UI elements")]
    [SerializeField] private List<TemplateButton> _templateButtons;

    public static TemplateEditor Instance
    {
        get;
        private set;
    }

    public void ConfigureEditorScreen(int templateIndex)
    {
        _chosenTemplate = templateIndex;
        //clear list
        //fill list
    }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
