using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player:ITargetable
{
    public int playerId;

    public int Turn=0;

    // fields: 玩家拥有的场地列表上限为3个
    public List<Field> fields = new List<Field>(3);


    #region 卡牌列表
    // 卡牌列表
    // inDeckCards: 在牌库中的卡牌
    // inHandCards: 在手牌中的卡牌
    // inBattleFieldCards : 在战场上的卡牌（即在场上的卡牌）
    // inDamageCards: 在伤害区的卡牌
    // inDiscardCards: 在弃牌区中的卡牌
    // inSoulCards: 在灵魂区的卡牌


    public List<Card> inDeckCards = new List<Card>(),
        inHandCards = new List<Card>(),
        inDamageCards = new List<Card>(),
        inBattleFieldCards = new List<Card>();

    public List<Card> inSoulCards = new List<Card>();
    public List<Card> inDiscardCards = new List<Card>();
    #endregion


    public List<Card> inFieldCards => 
        fields.SelectMany(f => (f.card != null ? new[] { f.card } : 
        Enumerable.Empty<Card>()).Concat(f.soulCards ?? new List<Card>())).ToList();



    public List<Card> AllCards
    {
        get
        {
            return inDeckCards.Concat(inHandCards).Concat(inFieldCards).Concat(inSoulCards).Concat(inDamageCards).Concat(inDiscardCards).ToList();
        }
    }

    public Player(int playerId)
    {
        this.playerId = playerId;
        for (int i = 0; i < 3; i++)
        {
            fields.Add(new Field(this));
        }
    }

}
