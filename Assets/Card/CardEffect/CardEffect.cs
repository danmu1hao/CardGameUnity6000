using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CardTypeEnum = CardEnums.CardTypeEnum;

public class CardEffect
{
    //用于记录触发器信息，比如攻击者是谁，这里采用依赖注入，每次进入效果必然传入
    public TriggerData triggerData;
    
    public int playerID => this.card.playerID;
    

    public Card card; // 拥有这个效果的卡牌

    #region varible
    public int effectID => cardEffectConfig.effectID;
    public string effectName => cardEffectConfig.effectName;
    public string effectText => cardEffectConfig.effectText;
    public CardTypeEnum CardTypeEnum; 
    public string cardStyle => cardEffectConfig.cardStyle;
    
    
    #endregion

    /// <summary>
    /// 一个效果管多个原子效果
    /// </summary>
    public CardEffectConfig cardEffectConfig;
    public readonly  AtomicEffect AtomicEffect;
    public readonly  Timing timing;
    public readonly  Condition condition;

    // 需要频繁更新
    public bool CanActive ;
    
    /// <summary>
    /// 前置效果代价支付 好麻烦。 如果确认发动则继续执行
    /// </summary>
    public List<AtomicEffect> preEffectList = new List<AtomicEffect>();

    /// <summary>
    /// 原子执行效果，只负责效果执行
    /// </summary>
    public List<AtomicEffect> subEffectList = new List<AtomicEffect>();
    
    public CardEffect(CardEffectConfig cardEffectConfig,Card card)
    {
        this.card = card;
        this.cardEffectConfig = cardEffectConfig;
        
        foreach (var atomicEffectConfig in cardEffectConfig.atomicEffectConfigList)
        {
            this.AtomicEffect = ReflactionSystem.FindClassByName<AtomicEffect>(atomicEffectConfig.effect);
            if (AtomicEffect == null)
            {
                AtomicEffect = new NoneType();
                Debug.Log("nonetype");
            }
            this.AtomicEffect.cardEffect=this;
        }

            
        //TODO
        this.timing = new Timing();
            
        this.condition = new Condition(SplitAndStr(cardEffectConfig.condition),this); 



        this.card = card;
            
        timing.LoadTiming(this.cardEffectConfig.timing);
        Debug.Log(card.name+cardEffectConfig.timing);
    }


    public string[] SplitAndStr(string str)
    {
        return str.Split("&&");
    }



    #region EffectActive    

    /// <summary>
    /// TODO 效果发动
    /// </summary>
    public async Task<bool> CardEffectAcitvePre()
    {
        // 记住 目前不确定如果pre需要连续选择好几次对象会咋样
        foreach (var atomicEffect in preEffectList)
        {
            if (await atomicEffect.ExecuteAsync()==false) return false;
        }

        return true;
    }
    public async Task<bool> ActiveCardEffect()
    {
        return true;
    }

    #endregion


    #region Effect

    

    #endregion
    






    // TODO CardEffect实现
    public bool CheckCardEffectAcitve()
    {
        return true;
    }

}
