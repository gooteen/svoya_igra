﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionItem : MonoBehaviour
{
    public int questionId;

    [SerializeField] private TMP_Text _questionPointsText;

    private string _questionPoints;

    public string ThemeName
    {
        get { return _questionPoints; }
        set
        {
            _questionPoints = value;
            _questionPointsText.text = _questionPoints;
        }
    }
}
