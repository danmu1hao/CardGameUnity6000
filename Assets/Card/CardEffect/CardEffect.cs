using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffect
{
    //用于记录触发器信息，比如攻击者是谁，这里采用依赖注入，每次进入效果必然传入
    public TriggerData triggerData;
    
    public int playerID => this.card.playerID;
    

    public Card card; // 拥有这个效果的卡牌

    #region varible
    

    public string effectDetail => effectConfig.effectDetail;
    public string effectDuration => effectConfig.effectDuration;
    public string extra => effectConfig.extra;
    public string effectText => effectConfig.effectText;

    #endregion

    /// <summary>
    /// 一个效果管多个原子效果
    /// </summary>
    public List<EffectConfig> effectConfigList;
    public AtomicEffect AtomicEffect;
    public Timing timing;
    public Condition condition;
    public Target target;


    /// <summary>
    /// 前置效果代价支付 好麻烦。 如果确认发动则继续执行
    /// </summary>
    public List<AtomicEffect> preEffectList = new List<AtomicEffect>();

    /// <summary>
    /// 原子执行效果，只负责效果执行
    /// </summary>
    public List<AtomicEffect> subEffectList = new List<AtomicEffect>();
    
    public CardEffect(List<EffectConfig> effectConfigList,Card card)
    {
        this.effectConfigList = effectConfigList;

        foreach (var effectConfig in effectConfigList)
        {

            this.AtomicEffect = ReflactionSystem.FindClassByName<AtomicEffect>(effectConfig.effect);
            if (AtomicEffect == null)
            {
                AtomicEffect = new NoneType();
                Debug.Log("nonetype");
            }
            this.AtomicEffect.cardEffect=this;
            
            //TODO
            this.timing = new Timing();
            
            this.condition = new Condition(SplitStr(effectConfig.condition),this); 
            //TODO
            this.target = new Target();


            this.card = card;
            
            timing.LoadTiming(this.effectConfig.timing);
            Debug.Log(card.name+effectConfig.timing);
        }

    }


    public string[] SplitStr(string str)
    {
        return str.Split("&&");
    }

    //检查条件是否满足，如果满足则执行    
    // public bool CheckConditionAndCost(TriggerData triggerData)
    // {
    //     //记录一下
    //
    // }
    //

    // TODO CardEffect实现
    public bool CanCardEffectAcitve()
    {
        return true;
    }


    public async Task<bool> CardEffectAcitvePre()
    {
        // 记住 效果发动是需要询问的
        foreach (var atomicEffect in preEffectList)
        {
            await atomicEffect.ExecuteAsync();
        }
    }

    /// <summary>
    /// TODO 出示确认
    /// </summary>
    async Task<bool> CardEffectAcitvePre_ENSURE()
    {
        await Time.time(1);
        
    }
    
    public async Task CardEffectAcitve()
    {
        // PayCost();
        // Debug.Log("代价支付完毕");
        await ExecuteAsync();
    }
    //  async Task ExecuteAsync()
    // {
    //     Debug.Log($"[Effect] 执行：{effectConfig.effectText}");
    //
    //     List<ITargetable> targetList=Target.FindTarget(effectConfig.target, card, triggerData);
    //     
    //     AtomicEffect.EffectExecute();        
    //
    //     await Task.Delay(500); // 动画等待时间
    // }
}
