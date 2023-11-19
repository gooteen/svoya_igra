using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerList", menuName = "PlayerList")]
public class PlayersSO : ScriptableObject
{
    public List<PlayerData> players;
}

[System.Serializable]
public class PlayerData
{
    public int score;
    public string name;

    public PlayerData(int newScore, string newName)
    {
        score = newScore;
        name = newName;
    }
}
