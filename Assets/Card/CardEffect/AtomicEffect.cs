using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ExtraInfoEnum=CardEnums.ExtraInfoEnum;
public abstract class AtomicEffect
{
    #region  effectOrigin
    public AtomicEffectConfig AtomicEffectConfig;
    public int AtomicEffect_ID => AtomicEffectConfig.effectID;
    public string AtomicEffect_Name => AtomicEffectConfig.effectName;
    public string AtomicEffect_Text => AtomicEffectConfig.internalDesc;
    public string AtomicEffect_Detail => AtomicEffectConfig.effectDetail;
    
    public string AtomicEffect_Duration => AtomicEffectConfig.effectDuration;

    #endregion
    public Player targetPlayer => target.targetPlayer;
    public Card card => cardEffect.card;
    
    public List<ExtraInfoEnum> AtomicEffect_Extra_Info;

    public Target target;
    public CardEffect cardEffect;
    
    public List<Card> targetCards => target.targetCards;

    
    int effectNum;  
    public List<Card> targetCardList=new List<Card>();
    
    public TriggerData triggerData =>cardEffect.triggerData;
    protected AtomicEffect()
    {
    }


    public AtomicEffect(AtomicEffectConfig atomicEffectConfig, CardEffect cardEffect)
    {
        this.cardEffect = cardEffect;
        this.AtomicEffectConfig = atomicEffectConfig;
        string[] extraSplit =
            atomicEffectConfig.extra.Split("&&");
        foreach (var extraStr in extraSplit)
        {
            this.AtomicEffect_Extra_Info.Add(CardEnums.TryGetEnum<ExtraInfoEnum>(extraStr));
        }
        
    }


    async Task<bool> FindTarget()
    {
        return await target.GetValidTargets();
    }

    /// <summary>
    /// 毕竟效果执行需要找对象，如果不选的话那还是算取消，但是如果支付代价又是必须执行
    /// </summary>
    /// <returns></returns>
    public async Task<bool> EffectExecute()
    {
        bool targetSuccess=
        await FindTarget();
        if(!targetSuccess)return false;
        
        if (cardEffect == null)
        {
             LogCenter.LogError($"{GetType().Name}: cardEffect 为空");
            return false;
        }

        if (cardEffect.card == null)
        {
             LogCenter.LogError($"{GetType().Name}: cardEffect.card 为空");
            return false;
        }

        if (cardEffect.card.name == null)
        {
             LogCenter.LogError($"{GetType().Name}: cardEffect.card.name 为空");
            return false;
        }

        
         LogCenter.Log($"执行效果 {GetType().Name}");
        await OnExecute();

        return true;
        
    }
    // TODO 本方法还有待确认
    protected abstract Task OnExecute();
}