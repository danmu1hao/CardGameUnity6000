using System.Collections.Generic;
using System.Threading.Tasks;

public class destory : AtomicEffect
{
    protected override async Task OnExecute()
    {
        foreach (var card in targetCardList)
        {
            BattleSystem.instance.DestroyFieldCard(card);
        }
        
    }
}
