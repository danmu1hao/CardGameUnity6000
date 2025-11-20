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
    public long AtomicEffect_ID => AtomicEffectConfig.effectID;
    public string AtomicEffect_Name => AtomicEffectConfig.effectName;
    public string AtomicEffect_Text => AtomicEffectConfig.internalDesc;
    public string AtomicEffect_Detail => AtomicEffectConfig.effectDetail;
    
    public string AtomicEffect_Duration => AtomicEffectConfig.effectDuration;

    #endregion
    public Player player=>card.player;

    public Player targetPlayer => target.targetPlayer;
    public Card card => cardEffect.card;


    public CardEffect cardEffect;
    public Target target;
    public List<ExtraInfoEnum> AtomicEffect_Extra_Info=new List<ExtraInfoEnum>();
    
    public List<Card> targetCards => target.targetCards;


    public TriggerData triggerData =>cardEffect.triggerData;
    public AtomicEffect()
    {
    }


    public void AtomicEffectImportData(AtomicEffectConfig atomicEffectConfig, CardEffect cardEffect)
    {
        this.cardEffect = cardEffect;
        this.AtomicEffectConfig = atomicEffectConfig;

        string[] targetStr = splitStr(atomicEffectConfig.target);
        Debug.Log("targetTest"+atomicEffectConfig.effectName+atomicEffectConfig.target);
        target = new Target(this, targetStr.ToList());        

        Debug.Log("测试对象是否正确"+atomicEffectConfig.target);
        string[] extraSplit = splitStr(atomicEffectConfig.extra);
        foreach (var extraStr in extraSplit)
        {
            this.AtomicEffect_Extra_Info.Add(CardEnums.TryGetEnum<ExtraInfoEnum>(extraStr));
        }
        
    }

    string[] splitStr(string str)
    {
        return str.Split("&&");
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
        if (target == null)
        {
            Debug.LogWarning("对象为空");
        }
        Debug.Log("找寻对象");
        bool targetSuccess=
        await FindTarget();
        if(!targetSuccess)return false;
        
        if (cardEffect == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect 为空");
            return false;
        }

        if (cardEffect.card == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect.card 为空");
            return false;
        }

        if (cardEffect.card.name == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect.card.name 为空");
            return false;
        }

        
        Debug.Log($"执行效果 {GetType().Name}");
        await OnExecute();

        return true;
        
    }
    // TODO 本方法还有待确认
    protected abstract Task OnExecute();
}