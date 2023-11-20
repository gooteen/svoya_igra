using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionItem : MonoBehaviour
{
    public int questionId;
    public int themeId;
    public GameObject marker;

    [SerializeField] private TMP_Text _questionPointsText;

    private string _questionPoints;

    public string QuestionValue
    {
        get { return _questionPoints; }
        set
        {
            _questionPoints = value;
            _questionPointsText.text = _questionPoints;
        }
    }

    public void ActivateMarker()
    {
        marker.SetActive(true);
    }

    public void DeleteQuestions()
    {
        TemplateEditor.Instance.ClearQuestionColumn(questionId);
    }

    public void OpenQuestionEditor()
    {
        QuestionEditor.Instance.ConfigureQuestionEditor(themeId, questionId);
    }
}
