using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Card :ITargetable
{
    public Player player;

    public Field field;
    
    public Sprite cardImage;
    
    public int playerID => player.playerId;

    public CardConfig cardConfig;
    
    public int id;
    public string name;
    public int atk;

    // 卡牌的费用 灵魂或者血祭
    public int soulCost;
    public int sacrificeCost;
    public List<Card> soulTargets = new List<Card>();
    public List<Card> sacrificeTargets=new List<Card>();
    
    
    public CardEnums.CardType cardType
    {
        get
        {
            if (Enum.TryParse<CardEnums.CardType>(cardConfig.type, out var result))
                return result;
            else
                throw new Exception($"无效的 CardType: {cardConfig.type}");
        }
    }

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
        
        Sprite sprite = Resources.Load<Sprite>("CardImage/" + this.id.ToString());
        if (sprite!= null)
        {
            this.cardImage = sprite;
        }

        
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

    public static Card QuickCardSample()
    {
        return new Card();
    }
    
    public List<CardEffect> cardEffectList=new List<CardEffect>();

    
    public bool cancelFight;

    #region  CardState


    /// 返回卡牌的玩家的对应列表
    /// 注意：如果是在field上单独判断
    public List<Card> GetCardListByState(CardEnums.CardStateEnum state,Field targetField=null)
    {
        switch (state)
        {
            case CardEnums.CardStateEnum.InDeck:
                return player.inDeckCards;

            case CardEnums.CardStateEnum.InHand:
                return player.inHandCards;

            case CardEnums.CardStateEnum.InDamage:
                return player.inDamageCards;

            case CardEnums.CardStateEnum.InDiscard:
                return player.inDiscardCards;

            case CardEnums.CardStateEnum.InSoul:
                if (targetField!=null)
                {
                    return targetField.soulCards;
                }
                if (this.field != null)
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


    public CardEnums.CardStateEnum state;


    public void Move(CardEnums.CardStateEnum newState,Field field=null)
    {
        Leave(this.state);
        MoveTo(newState,field);
        Debug.Log("move from "+state);
        Debug.Log("move to "+newState);
        this.state = newState;
    }
    
    /// <summary>
    ///  离开的时候，如果是在场上，照理来说必然会记录field，所以不用传入field
    /// </summary>
    /// <param name="currentState"></param>
    void Leave(CardEnums.CardStateEnum currentState)
    {
        if (currentState==CardEnums.CardStateEnum.InField && field!=null)
        {
            Debug.Log("field卡牌移除"+field.fieldIndex);
            field.card =  null;
        }
        List<Card> cardList = GetCardListByState(currentState,field);
        if (cardList != null)
        {
            cardList.Remove(this);
        }
    }

    void MoveTo(CardEnums.CardStateEnum newState,Field field=null)
    {
        if (newState==CardEnums.CardStateEnum.InField && field!=null)
        {
            Debug.LogError("add to field");
            this.field = field;
            field.card = this;
        }  else if(newState==CardEnums.CardStateEnum.InField && field==null) {
            Debug.LogError("field is null");
        }
        List<Card> cardList = GetCardListByState(newState);
        if (cardList != null)
        {
            cardList.Add(this);
        }
        
    }

    
    

    #endregion


}
