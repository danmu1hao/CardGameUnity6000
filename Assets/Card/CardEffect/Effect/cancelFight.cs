using System.Collections.Generic;

public class cancel_Fight: AtomicEffect
{
    public override void EffectExecute()
    {
        base.EffectExecute();
        cardEffect.effectTarget.cancelFight = true;
    }
}
