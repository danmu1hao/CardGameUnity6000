using UnityEngine;

public class discard_card: AtomicEffect
{
    public override void EffectExecute()
    {
        base.EffectExecute();
        Debug.Log("丢卡效果触发");
        //TODO 给对手抽卡？

        Debug.Log($"cardEffect.card.name = {cardEffect.card.name}");
        BattleSystem.instance.DrawCard(cardEffect.card.player);
    }
}