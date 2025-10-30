using System.Collections.Generic;

public abstract class Effect
{
    public CardEffect cardEffect;
    int effectNum;  
    public List<Card> targetCardList=new List<Card>();
    // 对象啥的自己去找
    public virtual void EffectExecute()
    {

        foreach (var itarget in cardEffect.effectTargetList)
        {
            Card card=itarget as Card;
            if (card!=null)
            {
                targetCardList.Add(card);
            }
        }
        
    }
}