using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerItem : MonoBehaviour
{
    public TMP_InputField input_name;
    public TMP_InputField input_score;
    public TMP_Text text_score;
    public TMP_Text text_name;
    public int index;

    public void UpdatePlayerName()
    {
        GameController.Instance.Players.players[index].name = input_name.text;
    }

    public void UpdatePlayerScore()
    {
        int parsedValue = 0; 

        try
        {
            parsedValue = int.Parse(input_score.text);
            GameController.Instance.Players.players[index].score = parsedValue;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }


    public void DeletePlayer()
    {
        GameController.Instance.Players.players.RemoveAt(index);
        GameController.Instance.ClearPlayerList();
        GameController.Instance.FillPlayerList();
    }
}
