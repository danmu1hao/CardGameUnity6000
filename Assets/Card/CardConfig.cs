using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public struct CardConfig
{
    public int id;
    public string name;
    public int atk;
    // 卡牌的费用 灵魂或者血祭
    public int soulCost;
    public int sacrificeCost;
    
    public string tribe;
    public string type;

    public string[]  effectDescription;
    public CardConfig(string cardData)
    {
        var parts = cardData.Split(',');
        Debug.Log(parts.Length);

        id = int.TryParse(parts[0], out var tempId) ? tempId : 0;
        name = parts[1];
        atk = int.TryParse(parts[2], out var tempAtk) ? tempAtk : 0;

        soulCost = int.TryParse(parts[3], out var tempSoul) ? tempSoul : -1;
        sacrificeCost = int.TryParse(parts[4], out var tempSac) ? tempSac : -1;

        tribe = parts[5];
        type = parts[6];
        effectDescription=parts[7].Split(';');


    }

}