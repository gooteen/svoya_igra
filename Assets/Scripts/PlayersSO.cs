using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerList", menuName = "PlayerList")]
public class PlayersSO : ScriptableObject
{
    public List<PlayerData> players;
}

[System.Serializable]
public class PlayerData : IComparable<PlayerData>
{
    public int score;
    public string name;

    public PlayerData(int newScore, string newName)
    {
        score = newScore;
        name = newName;
    }

    public int CompareTo(PlayerData other)
    {
        if (this.score > other.score) return -1;
        if (this.score == other.score) return 0;
        return 1;
    }
}
