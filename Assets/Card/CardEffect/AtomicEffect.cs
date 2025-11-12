using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AtomicEffect
{
    public CardEffect cardEffect;
    int effectNum;  
    public List<Card> targetCardList=new List<Card>();
    // 
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