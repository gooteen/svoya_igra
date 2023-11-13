using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThemeItem : MonoBehaviour
{
    public int themeId;

    [SerializeField] private TMP_Text _templateNameText;
    [SerializeField] private Transform _container_questions;

    private string _themeName;

    public string ThemeName
    {
        get { return _themeName; }
        set
        {
            _themeName = value;
            _templateNameText.text = _themeName;
        }
    }
}

