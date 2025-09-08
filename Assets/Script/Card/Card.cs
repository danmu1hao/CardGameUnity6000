using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Card :ITargetable
{
    public Player player;

    public Field field;
    
    public int playerID => player.playerId;

    public CardConfig cardConfig;
    
    public int id;
    public string name;
    public int atk;

    // 卡牌的费用 灵魂或者血祭
    public int soulCost;
    public int sacrificeCost;

    public string type =>cardConfig.type;
    public GameObject cardModel;

    public bool hasAttacked;

    public Card(Player player, CardConfig cardConfig)
    {
        this.player = player;
        this.id = cardConfig.id;
        this.name = cardConfig.name;
        this.atk = cardConfig.atk;
        this.sacrificeCost = cardConfig.sacrificeCost;
        this.soulCost = cardConfig.soulCost;

        this.cardConfig = cardConfig;
        //字符串解析器
        CardEffect cardEffect = new CardEffect(cardConfig.effectConfig,this);
        cardEffectList.Add(cardEffect);
    }

    public Card()
    {
        cardConfig = new CardConfig();
    }
    /*public static Card TestCard()
    {
        CardConfig cardConfig = new CardConfig(1, "Test Card", 1, 1);
        return new Card(BattleSystem.Player1 ,cardConfig);
    }*/
    
    public List<CardEffect> cardEffectList=new List<CardEffect>();
    public List<CardEffect> canEffectList = new List<CardEffect>();
    
    public bool cancelFight;

    public enum CardStateEnum
    {
        InDeck,InHand,InBattle,InDamage,InDiscard,InSoul
    }


    // 返回卡牌的玩家的对应列表
    public List<Card> GetCardListByState(CardStateEnum state)
    {
        switch (state)
        {
            case CardStateEnum.InDeck:
                return player.inDeckCards;

            case CardStateEnum.InHand:
                return player.inHandCards;

            case CardStateEnum.InBattle:
                return player.inFieldCards;

            case CardStateEnum.InDamage:
                return player.inDamageCards;

            case CardStateEnum.InDiscard:
                return player.inDiscardCards;

            case CardStateEnum.InSoul:
                if (field != null)
                {
                    // ✅ 如果指定了某个场地，返回该场地的灵魂卡
                    return field.soulCards;
                }
                else
                {

                    // ✅ 没指定场地，则返回所有灵魂卡合并列表
                    List<Card> souls = new List<Card>();
                    foreach (var f in player.fields)
                    {
                        souls.AddRange(f.soulCards);
                    }
                    return souls;
                }
            default:
                return null;
        }
    }


    public CardStateEnum CardState;


    public void Move(CardStateEnum newState)
    {

        var fromList = GetCardListByState(CardState);
        var toList = GetCardListByState(newState);

        fromList?.Remove(this);
        toList?.Add(this);

        CardState = newState;
    }


    public void Move(Field field, CardStateEnum newCardState)
    {

        // 判断状态


        var fromList = GetCardListByState(CardState);
        fromList?.Remove(this);
        if(newCardState== CardStateEnum.InBattle)
        {
            field.card = this;
        }
        else if(newCardState == CardStateEnum.InSoul)
        {
            field.soulCards.Add (this);
        }


        CardState = newCardState;
    }



}
