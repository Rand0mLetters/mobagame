using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMatchData
{
    public int xp;
    public float health;
    public string playerId;
    public int gold = 750;
    public MatchAttackData attackData;
    public List<Item> inventory;

    public PlayerMatchData() {
        inventory = new List<Item>();
    }
}
