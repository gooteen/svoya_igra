using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionItem : MonoBehaviour
{
    public int questionId;
    public int themeId;
    public GameObject marker;
    public bool isChecked;

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
        isChecked = true;
    }

    public void OpenQuestionWindow()
    {
        GameController.Instance.SetIndices(themeId, questionId);
        GameController.Instance.ConfigureQuestionWindow(0);
        GameController.Instance.PanelGameScreen.SetActive(false);
        GameController.Instance.PanelQuestionScreen.SetActive(true);
    }

    public void DeleteQuestions()
    {
        TemplateEditor.Instance.ClearQuestionColumn(questionId);
    }

    public void OpenQuestionEditor()
    {
        QuestionEditor.Instance.ConfigureQuestionEditor(themeId, questionId);
    }

    private void Start()
    {
        isChecked = false;
    }
}
