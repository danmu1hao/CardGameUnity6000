using System.Collections.Generic;

public class destory : Effect
{
    public override void EffectExecute()
    {
        base.EffectExecute();

        foreach (var card in targetCardList)
        {
            BattleSystem.instance.DestroyFieldCard(card);
        }
        
    }
}
