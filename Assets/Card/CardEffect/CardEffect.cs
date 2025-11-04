using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardEffect
{
    public EffectConfig effectConfig;
    public Card card; // 拥有这个效果的卡牌

    #region MyRegion
    

    public string effectDetail => effectConfig.effectDetail;
    public string effectDuration => effectConfig.effectDuration;
    public string extra => effectConfig.extra;
    public string effectText => effectConfig.effectText;

    #endregion

    public Card effectTarget;
    public Effect effect;
    public Condition condition;
    public Target target;
    public Cost cost;
    public Timing timing;

    public  List<ITargetable> effectTargetList = new List<ITargetable>();
    
    public CardEffect(EffectConfig effectConfig,Card card)
    {
        this.effectConfig = effectConfig;
        
        this.effectTarget = new Card();
        
        this.effect = ReflactionSystem.FindClassByName<Effect>(effectConfig.effect);
        if (effect == null)
        {
            effect = new NoneType();
            Debug.Log("nonetype");
        }
        this.effect.cardEffect=this;
        
        this.condition = new Condition(SplitStr(effectConfig.condition),this); 
        //TODO
        this.target = new Target();
        //TODO
        this.cost = new Cost(SplitStr(effectConfig.cost_type),this);
        //TODO
        this.timing = new Timing();

        this.card = card;
        
        timing.LoadTiming(this.effectConfig.timing);
        Debug.Log(card.name+effectConfig.timing);
    }


    public string[] SplitStr(string str)
    {
        return str.Split("&&");
    }
    //用于记录触发器信息，比如攻击者是谁
    public TriggerData triggerData;
    //检查条件是否满足，如果满足则执行    
    public bool CheckConditionAndCost(TriggerData triggerData)
    {
        //记录一下
        this.triggerData = triggerData;
        
        // 检查条件是否满足
        foreach (var conditionStr in condition.conditionList)
        {
            Debug.Log(conditionStr);
            //所有条件必须全部满足
            if (!FieldResolver.Resolver(conditionStr,this.card,triggerData))
            {
                
                return false;
            }
        }
        //检查是否有支付cost的条件

        CheckCost();
        
        return true;
    }

    void CheckCost()
    {
        //waiting
        
    }
    


    void CheckTarget()
    {
        
    }
    
    public async Task Start()
    {
        // PayCost();
        // Debug.Log("代价支付完毕");
        await ExecuteAsync();
    }
     async Task ExecuteAsync()
    {
        Debug.Log($"[Effect] 执行：{effectConfig.effectText}");

        List<ITargetable> targetList=Target.FindTarget(effectConfig.target, card, triggerData);
        
        effect.EffectExecute();        

        await Task.Delay(500); // 动画等待时间
    }
}
