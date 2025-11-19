using System.Threading.Tasks;
using UnityEngine;

public class discard_card : AtomicEffect
{
    protected override async Task OnExecute()
    {

         LogCenter.Log("丢卡效果触发");
         LogCenter.Log($"cardEffect.card.name = {cardEffect.card.name}");
        BattleSystem.instance.DrawCard(cardEffect.card.player);
    }
}