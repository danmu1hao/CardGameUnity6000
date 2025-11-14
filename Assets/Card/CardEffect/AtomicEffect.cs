using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtomicEffect
{
    #region  effectOrigin
    public int AtomicEffect_ID;
    public string AtomicEffect_Name;
    public string AtomicEffect_Text;
    public string AtomicEffect_Detail;
    
    public string AtomicEffect_Timing;
    public string AtomicEffect_Extra_Info;
    

    #endregion
    public Target target;
    public CardEffect cardEffect;
    public List<Card> targetCards => target.targetCards;
    public Player targetPlayer => target.targetPlayer;
    int effectNum;  
    public List<Card> targetCardList=new List<Card>();

    public AtomicEffect(List<EffectConfig> effectConfigList, Card card)
    {
        
    }


    public void FindTarget()
    {
        target.GetValidTargets();
    }

    public virtual void EffectExecute()
    {
        if (cardEffect == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect 为空");
            return;
        }

        if (cardEffect.card == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect.card 为空");
            return;
        }

        if (cardEffect.card.name == null)
        {
            Debug.LogError($"{GetType().Name}: cardEffect.card.name 为空");
            return;
        }
        
        Debug.Log($"执行效果 {GetType().Name}");

        /*foreach (var itarget in cardEffect.effectTargetList)
        {
            Card card=itarget as Card;
            if (card!=null)
            {
                targetCardList.Add(card);
            }
        }*/
        
    }
}