using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public static EffectSystem Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public GameObject EffectPanelUI;
    public List<CardEffect> effectList=new List<CardEffect>();
    public async Task CheckEffectWithTiming(Timing.TimingList timing,TriggerData triggerData)
    {
        effectList.Clear();
        // 第一步：查找符合时点的卡牌效果
        foreach (var card in BattleSystem.Instance.Player1.AllCards.Concat(BattleSystem.Instance.Player2.AllCards))
        {
            foreach (var effect in card.cardEffectList)
            {
                if (effect.timing.timingEnum == timing)
                {
                    Debug.Log("检测到可发动卡牌"+card.name+" ");
                    effectList.Add(effect);
                }
            }
        }

        List<Card> cards=new List<Card>();

        Debug.Log(timing.ToString()+"时点检测完毕"+effectList.Count);
        
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            var effect = effectList[i];
            if (!effect.CheckConditionAndCost(triggerData))
            {
                effectList.RemoveAt(i);
            }
        }

        foreach (var effect in effectList)
        {
            //因为后面要用所以清空
            effect.card.canEffectList.Clear();
        }
        
        Debug.Log(timing.ToString()+"条件检测完毕"+effectList.Count);
        foreach (var effect in effectList)
        {
            effect.card.canEffectList.Add(effect);
            cards.Add(effect.card);
        }
        
        // 第二步：确认是否支付cost  
        
        if (cards.Count>0)
        {
            EffectPanelUI.SetActive(true);
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            EffectPanelUI.GetComponent<EffectCardsPanel>().ShowEffectCards(cards, tcs);
            await tcs.Task;
        }
        // 所有卡牌效果入队完毕后，依次执行
        await ExecuteAllEffectsAsync();
        /*await ExecuteAllEffectsAsync();*/
    }

    #region  EffectSys

    
    public List<CardEffect> effectQueue = new();

    public void EnQueue(CardEffect cardEffect)
    {
        effectQueue.Add(cardEffect);
    }
    
    private async Task ExecuteAllEffectsAsync()
    {
        Debug.Log($"[EffectSystem] 开始执行效果队列，共 {effectQueue.Count} 个效果");

        while (effectQueue.Count > 0)
        {
            CardEffect cardEffect = effectQueue[0];
            effectQueue.RemoveAt(0);

            Debug.Log($"[EffectSystem] 准备执行效果：{cardEffect.effectConfig.effectText}，来自卡牌：{cardEffect.card.name}");

            await ExecuteEffectAsync(cardEffect);

            Debug.Log($"[EffectSystem] 完成效果：{cardEffect.effectConfig.effectText}");
        }

        Debug.Log($"[EffectSystem] 所有效果执行完毕");
    }

    private async Task ExecuteEffectAsync(CardEffect cardEffect)
    {

        await ShowEffectAnimation(cardEffect);

        await cardEffect.Start();

    }

    private async Task ShowEffectAnimation(CardEffect cardEffect)
    {
        Debug.Log($"[EffectSystem] 播放效果动画：{cardEffect.effectConfig.effectText}（卡：{cardEffect.card.name}）");
        await Task.Delay(1000); // 模拟动画等待
    }
    
    #endregion
    

}
