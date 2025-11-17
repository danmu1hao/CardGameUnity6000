using System.Collections.Generic;
using System.Threading.Tasks;

// 增减伤害区

public class add_card_damage : AtomicEffect
{
    
    public override async Task<bool> EffectExecute()
    {
        base.EffectExecute();

        // foreach (var targetSingle in target)
        // {
        //     Card card = targetSingle as Card;
        //     card.Move(Card.CardStateEnum.InDamage);
        // }

        
    }
}
