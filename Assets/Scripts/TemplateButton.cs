using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemplateButton : MonoBehaviour
{
    public int templateId;

    [SerializeField] private TMP_Text _templateNameText;

    private string _templateName;

    public string TemplateName
    {
        get { return _templateName; }
        set 
        {
            _templateName = value;
            _templateNameText.text = _templateName; 
        }
    }

    public void SetUpEditor()
    {
        TemplateEditor.Instance.ConfigureEditorScreen(templateId);
    }

    public void DeleteTemplate()
    {
        Controller.Instance.GameData.data.userTemplates.RemoveAt(templateId);
        Controller.Instance.ConfigureTemplateList();
    }
}
