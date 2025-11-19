using System;

using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class Card :IClassResolver
{
    public readonly Player player;

    public Field field;
    
    public Sprite cardImage;
    
    public int playerID => player.playerId;

    public CardConfig CardConfig;
    public List<CardEffectConfig> EffectConfig;
    
    public int id;
    public string name;
    public int atk;

    // 卡牌的费用 灵魂或者血祭
    public readonly  int soulCost;
    public readonly  int sacrificeCost;
    
    public List<CardEffect> cardEffectList=new List<CardEffect>();
    
    public List<Card> soulTargets = new List<Card>();
    public List<Card> sacrificeTargets=new List<Card>();
    
    public CardEnums.CardTypeEnum CardTypeEnum
    {
        get
        {
            if (Enum.TryParse<CardEnums.CardTypeEnum>(CardConfig.type, out var result))
                return result;
            else
                throw new Exception($"无效的 CardType: {CardConfig.type}");
        }
    }

    public GameObject cardModel;

    #region constructor

    public Card(Player player, CardConfig cardConfig)
    {
        this.player = player;
        this.id = cardConfig.id;
        this.name = cardConfig.name;
        this.atk = cardConfig.atk;
        this.sacrificeCost = cardConfig.sacrificeCost;
        this.soulCost = cardConfig.soulCost;

        this.CardConfig = cardConfig;

        Sprite sprite = Resources.Load<Sprite>("CardImage/" + this.id.ToString());
        if (sprite!= null)
        {
            this.cardImage = sprite;
        }

        
        // TODO 之后再确认一下，这里给卡牌添加效果
        // 卡牌获取自己所有的卡牌效果id
        List<int> effectConfigIDList = GameManager.instance.cardIDEffectConfigDict[id];
        foreach (int cardeffectID in effectConfigIDList)
        {
            CardEffectConfig effectConfigList = GameManager.instance.effectIDEffectConfigDict[cardeffectID];
            CardEffect cardEffect = new CardEffect(effectConfigList,this);
            cardEffectList.Add(cardEffect);
        }
        

        
    }

    public Card()
    {
        CardConfig = new CardConfig();
    }
    /*public static Card TestCard()
    {
        CardConfig cardConfig = new CardConfig(1, "Test Card", 1, 1);
        return new Card(BattleSystem.Player1 ,cardConfig);
    }*/


    #endregion

    public static Card QuickCardSample()
    {
        return new Card();
    }


    #region EffectVarible
    
    public bool cancelFight;
    public bool hasAttacked;

    

    #endregion

    #region  CardState And Move
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

            case CardEnums.CardStateEnum.InCemetery:
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
         LogCenter.Log("move from "+state);
         LogCenter.Log("move to "+newState);
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
             LogCenter.Log("field卡牌移除"+field.fieldIndex);
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
             LogCenter.LogError("add to field");
            this.field = field;
            field.card = this;
        }  else if(newState==CardEnums.CardStateEnum.InField && field==null) {
             LogCenter.LogError("field is null");
        }
        List<Card> cardList = GetCardListByState(newState);
        if (cardList != null)
        {
            cardList.Add(this);
        }
        
    }

    
    

    #endregion

    #region Resolvers

    public string TryResolveCard(string resolveContent)
    {
         LogCenter.Log(resolveContent);
        return null;
    }

    #endregion

}
