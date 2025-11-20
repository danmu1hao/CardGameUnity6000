using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TimingEnum=CardEnums.TimingEnum;

public class EffectSystem : QuickInstance<EffectSystem>
{

    // Start is called before the first frame update
    
    public GameObject EffectPanelUI;

    #region TimingEffectDict

    public void SubscribeAllEffect()
    {
        foreach (var card in BattleSystem.instance.allCardsInBattle)
        {
            foreach (var cardEffect in card.cardEffectList)
            {
                SubscribeReaction(cardEffect.timing.timingEnum, cardEffect);
            }
        }
    }
    
    public static Dictionary<TimingEnum,List<CardEffect>> timingEffectTriggerDict=
        new Dictionary<TimingEnum,List<CardEffect>>();
    /// <summary>
    /// 注册一个反应函数，根据 TimingEnum 归类存储
    /// </summary>
    public static void SubscribeReaction(TimingEnum timingEnum, CardEffect cardEffect)
    {
        // 如果没有这个 key，先创建一个空的 List
        if (!timingEffectTriggerDict.TryGetValue(timingEnum, out var list))
        {
            list = new List<CardEffect>();
            timingEffectTriggerDict[timingEnum] = list;
        }

        // 避免重复添加
        if (!list.Contains(cardEffect)){

            list.Add(cardEffect);}
        else
        {
            Debug.LogWarning("重复添加了效果");
        }
    }
    /// <summary>
    /// 取消注册
    /// </summary>
    public static void UnsubscribeReaction(TimingEnum timingEnum, CardEffect cardEffect)
    {
        if (timingEffectTriggerDict.TryGetValue(timingEnum, out var list))
        {
            list.Remove(cardEffect);
        
            // 如果列表空了，可以选择移除 key（可选）
            if (list.Count == 0)
                timingEffectTriggerDict.Remove(timingEnum);
        }
    }

    #endregion
 
    
    #region System CoreLogic
    /// <summary>
    /// 唯一对外界暴露的方法，第一启动点，启动以后自己转
    /// </summary>
    public async Task TimingTrigger(CardEnums.TimingEnum timing, TriggerData triggerData)
    {
        Debug.Log("时点触发 "+timing.ToString());
        await PlayerAcitveEffects(timing, triggerData);
        await ProcessChain();
    }

    private bool isActiving = false;
    // 连锁处理循环  只负责执行
    async Task ProcessChain()
    {
        if (isActiving)
        {
            return;
        }
        Debug.Log("效果开始执行");
        isActiving = true;
        while (_chainStack.Count > 0)
        {
            // 取出最上层的小栈，看看是不是空
            var currentEffect = _chainStack.Pop();

            await ExecuteEffectAsync(currentEffect);
            await ShowEffectAnimation(currentEffect);
            //这里还有新的检测
        }

        isActiving = false;
    }

    #endregion
    
        
    #region  check
    
    List<CardEffect> GetCanActiveCardEffect(TimingEnum timing, TriggerData triggerData)
    {
        
        List<CardEffect> effectList =timingEffectTriggerDict[timing];
        
        List<CardEffect> canEffectList=new List<CardEffect>();
        foreach (var effect in effectList)
        {
            //不满足条件
            if(CheckSingleEffect(effect,triggerData))canEffectList.Add(effect);
        }
        return canEffectList;
    }
    


    bool CheckSingleEffect(CardEffect effect, TriggerData triggerData)
    {
        // 1.传入当前发生事件
        effect.ImportTriggerData(triggerData);
        
        if (effect.AtomicEffectList is NoneType)
        {
            return false;
        }

        // 2. 条件检测
        if (!CheckCondition(effect)) return false;



        return true;
    }


    bool CheckCondition(CardEffect effect)
    {
        return effect.condition.CheckConditionAndTarget(); // 你原本的方法
    }
    

    #endregion

    
    // 时点产生导致的效果检测
    // 处理玩家是否发动卡牌
    async Task PlayerAcitveEffects(CardEnums.TimingEnum timing, TriggerData triggerData)
    {
        // 注意，每个时点都是一个完整stack链条

        // 把所有的能发动效果的卡给我
        List<CardEffect> cardEffects=GetCanActiveCardEffect(timing,triggerData);
        Debug.Log(cardEffects.Count()+"张卡可以发动");
        List<CardEffect> playerEffects,enemyEffects=new List<CardEffect>();
        // 分离出玩家和敌人的效果
        playerEffects = cardEffects.Where(x => x.card.player == BattleSystem.instance.currentPlayer).ToList();
        enemyEffects= cardEffects.Where(x => x.card.player !=  BattleSystem.instance.currentPlayer).ToList();
        // 按照顺序询问
        Debug.Log("询问玩家效果");
        await SelectCards_PopStacks(playerEffects);
        Debug.Log("询问敌方效果");
        await SelectCards_PopStacks(enemyEffects);
    }

    // 展示 → 选择 → 入队
    async Task SelectCards_PopStacks(List<CardEffect> cardEffect)
    {
        //其实发过去的是卡牌 准确说是卡牌的可执行的效果
        List<CardEffect> cardEffect_temp=new List<CardEffect>(cardEffect);
        List<Card> cards= cardEffect_temp.Select(x => x.card).ToList();
        //准确来说是可以发动的卡效果，每次支付要更新的
        while(cards.Count>0)
        {
            // 我可以补选的
            //TODO 这里目前没判断选哪个效果
            Debug.Log("等待选择效果");
            CardEffect selectEffect= await UIManager.instance.OpenSelectPanel(cardEffect_temp);
            await Task.Delay(50); // 等待 0.1 秒
            Debug.Log("选择效果返回 "+(selectEffect!=null?selectEffect.cardEffectConfig.effectText:"空"));
            if (selectEffect!=null)
            {

                // TODO 我需要移除那个选择的对象
                Debug.Log("选择了效果 "+selectEffect.cardEffectConfig.effectText);
                cards.Remove(selectEffect.card);
                
                // TODO 这是备忘录，在这里添加了效果
               Debug.Log("添加了效果"+selectEffect.cardEffectConfig.effectText);
                _chainStack.Push(selectEffect);
            }
            else
            {
                break;
            }
        }

    }


    #region  EffectSys 
    


    // 栈，存放效果
    Stack<CardEffect> _chainStack = new Stack<CardEffect>();

    // 效果执行宣言导致的效果检测

    
    int _effectTestCounter = 0; // 用于测试的计数器


    private async Task ExecuteEffectAsync(CardEffect cardEffect)
    {
        
        await cardEffect.Start();

    }

    private async Task ShowEffectAnimation(CardEffect cardEffect)
    {
        Debug.Log($"[EffectSystem] 播放效果动画：{cardEffect.cardEffectConfig.effectText}（卡：{cardEffect.card.name}）");
        await Task.Delay(1000); // 模拟动画等待
    }
    
    #endregion
    
    #region OldCheck
    
    // List<CardEffect> CheckEffectWithTiming(CardEnums.TimingEnum timing,TriggerData triggerData)
    //  {
    //      List<CardEffect> effectList = new List<CardEffect>();
    //      
    //      foreach (var card in BattleSystem.instance.Player1.AllCards.Concat(BattleSystem.instance.Player2.AllCards))
    //      {
    //          foreach (var effect in card.cardEffectList)
    //          {
    //              if (CheckSingleEffect(effect, triggerData, timing))
    //              {
    //                  Debug.Log("检测到可发动卡牌 " + card.name);
    //                  effectList.Add(effect);
    //              }
    //          }
    //      }
    //
    //      Debug.Log(timing.ToString()+"时点检测完毕"+effectList.Count);
    //      
    //      for (int i = effectList.Count - 1; i >= 0; i--)
    //      {
    //          var effect = effectList[i];
    //          if (!effect.CheckConditionAndCost(triggerData))
    //          {
    //              effectList.RemoveAt(i);
    //          }
    //      }
    //
    //      return effectList;
    //  }
    #endregion

}
