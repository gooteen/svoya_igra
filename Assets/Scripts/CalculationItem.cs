using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CalculationItem : MonoBehaviour
{
    public int id;
    public TMP_InputField inputField;
    public TMP_Text playerName;
    public TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown.value = 0;
    }
}
