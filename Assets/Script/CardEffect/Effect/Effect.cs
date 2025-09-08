using System.Collections.Generic;

public abstract class Effect
{
    public CardEffect cardEffect;
    int effectNum;  
    // 对象啥的自己去找
    public virtual void EffectExecute()
    {
        
    }
}