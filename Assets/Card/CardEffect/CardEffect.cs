using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CardTypeEnum = CardEnums.CardTypeEnum;

public class CardEffect
{
    //TODO 这个是否正确？
    public bool canActive_UI;
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
    public readonly  List<AtomicEffect> AtomicEffectList=new List<AtomicEffect>();
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
            AtomicEffect atomicEffect= ReflactionSystem.FindClassByName<AtomicEffect>(atomicEffectConfig.effect);
            if (atomicEffect == null)
            {
                atomicEffect = new NoneType();
                Debug.Log("nonetype");
            }
            else
            {
                atomicEffect.AtomicEffectImportData(atomicEffectConfig,this);
            }
            this.AtomicEffectList.Add(atomicEffect);
        }

            
        //TODO
        this.timing = new Timing(this.cardEffectConfig.timing);
            
        this.condition = new Condition(SplitAndStr(cardEffectConfig.condition),this); 



        this.card = card;
 
        Debug.Log(card.name+cardEffectConfig.timing);
    }


    public string[] SplitAndStr(string str)
    {
        return str.Split("&&");
    }

    
    public void ImportTriggerData(TriggerData triggerData)
    {
        this.triggerData = triggerData;
    }

    #region EffectActive

    public async Task<bool> Start()
    {
        Debug.Log("开始执行CardEffect"+AtomicEffectList.Count+"个原子效果");
        bool preSuccess = await CardEffectAcitvePre();
        if (!preSuccess) return false;
        return await ActiveCardEffect();
    }
    
    /// <summary>
    /// TODO 效果发动
    /// </summary>
    async Task<bool> CardEffectAcitvePre()
    {
        // 记住 目前不确定如果pre需要连续选择好几次对象会咋样
        foreach (var atomicEffect in preEffectList)
        {
            if (! await atomicEffect.EffectExecute()) return false;
        }

        return true;
    }
    async Task<bool> ActiveCardEffect()
    {
        foreach (var atomicEffect in AtomicEffectList)
        {
            await atomicEffect.EffectExecute();
        }
        return true;
    }

    #endregion



    






    // TODO CardEffect实现
    public bool CheckCardEffectAcitve()
    {
        return true;
    }

}
