using System.Threading.Tasks;
using UnityEngine;

public class draw_card : AtomicEffect
{
    protected override async Task OnExecute()
    {


        Debug.Log("抽卡效果触发");
        Debug.Log($"cardEffect.card.name = {cardEffect.card.name}");
        BattleSystem.instance.DrawCard(cardEffect.card.player);
    }
}