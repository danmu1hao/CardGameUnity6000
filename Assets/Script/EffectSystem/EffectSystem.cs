using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EffectSystem : QuickInstance<EffectSystem>
{
    public static EffectSystem instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public GameObject EffectPanelUI;

    /// <summary>
    /// 唯一对外界暴露的方法，第一启动点
    /// </summary>
    /// <param name="timing"></param>
    /// <param name="triggerData"></param>
    public async Task TimingTrigger(CardEnums.TimingList timing, TriggerData triggerData)
    {
        Debug.Log("时点触发 "+timing.ToString());
        await PlayerAcitveEffects(timing, triggerData);
        await ProcessChain();
    }
    
    // 处理玩家是否发动卡牌
    async Task PlayerAcitveEffects(CardEnums.TimingList timing, TriggerData triggerData)
    {
        // 注意，每个时点都是一个完整stack链条
        // 刷新
        _temp_chainStack = new Stack<CardEffect>();
        // 把所有的能发动效果的卡给我
        List<CardEffect> cardEffects=CheckEffectWithTiming(timing,triggerData);
        List<CardEffect> playerEffects,enemyEffects=new List<CardEffect>();
        // 分离出玩家和敌人的效果
        playerEffects = cardEffects.Where(x => x.card.player == BattleSystem.instance.currentPlayer).ToList();
        enemyEffects= cardEffects.Where(x => x.card.player !=  BattleSystem.instance.currentPlayer).ToList();
        // 按照顺序询问
        Debug.Log("询问玩家效果");
        await SelectCards_PopStacks(playerEffects);
        Debug.Log("询问敌方效果");
        await SelectCards_PopStacks(enemyEffects);
        
        //最后加入
        _chainStack.Push(_temp_chainStack);
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
            Debug.Log("选择效果返回 "+(selectEffect!=null?selectEffect.effectConfig.effectText:"空"));
            if (selectEffect!=null)
            {
                //之后加
                PayCost(selectEffect);
                // TODO 我需要移除那个选择的对象
                Debug.Log("选择了效果 "+selectEffect.effectConfig.effectText);
                cards.Remove(selectEffect.card);
                _temp_chainStack.Push(selectEffect);
            }
            else
            {
                break;
            }
        }

    }
    // 连锁处理循环  只负责执行
    async Task ProcessChain()
    {
        Debug.Log("效果开始执行");
        while (_chainStack.Count > 0)
        {
            // 取出最上层的小栈，看看是不是空
            var currentStack = _chainStack.Peek();
            if (currentStack.Count == 0)
            {
                // 小栈空了，说明该链条处理完毕，丢掉
                _chainStack.Pop();
                continue;
            }
            
            // 取出小栈里最新的效果
            // 看来不用我吐回去
            var currentEffect = currentStack.Pop();

            
            // 执行宣言，确认链条是否更新,这里我们放弃bool的传递了，我们直接用检测是否新效果stack为空
            _temp_chainStack.Clear();
            await DeclareEffect(currentEffect);

            if (_temp_chainStack.Count!=0)
            {
                // 这个效果先不管了，去下一轮吧
                currentStack.Push(currentEffect);
                _chainStack.Push(_temp_chainStack);
                
                continue;  
            }
            // 可以执行
            await ExecuteEffectAsync(currentEffect);
            await ShowEffectAnimation(currentEffect);
            //这里还有新的检测
        }
    }

    
    // 时点产生导致的效果检测

    #region  check
    
    async Task CardPayCost(CardEffect effect, TriggerData triggerData)
    {
        // 这里写支付费用的逻辑
        return ;
    }
    
   List<CardEffect> CheckEffectWithTiming(CardEnums.TimingList timing,TriggerData triggerData)
    {
        List<CardEffect> effectList = new List<CardEffect>();
        
        foreach (var card in BattleSystem.instance.Player1.AllCards.Concat(BattleSystem.instance.Player2.AllCards))
        {
            foreach (var effect in card.cardEffectList)
            {
                if (CheckSingleEffect(effect, triggerData, timing))
                {
                    Debug.Log("检测到可发动卡牌 " + card.name);
                    effectList.Add(effect);
                }
            }
        }

        Debug.Log(timing.ToString()+"时点检测完毕"+effectList.Count);
        
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            var effect = effectList[i];
            if (!effect.CheckConditionAndCost(triggerData))
            {
                effectList.RemoveAt(i);
            }
        }

        return effectList;
    }

    bool CheckSingleEffect(CardEffect effect, TriggerData triggerData, CardEnums.TimingList timing)
    {
        if (effect.effect is NoneType)
        {
            return false;
        }
        // 1. 时点检测
        if (!CheckTiming(effect, timing)) return false;

        // 2. 条件检测
        if (!CheckCondition(effect, triggerData)) return false;

        // 3. 支付费用检测
        if (!CheckCost(effect, triggerData)) return false;

        return true;
    }

    bool CheckTiming(CardEffect effect, CardEnums.TimingList timing)
    {
        return effect.timing.timingEnum == timing;
    }

    bool CheckCondition(CardEffect effect, TriggerData triggerData)
    {
        return effect.CheckConditionAndCost(triggerData); // 你原本的方法
    }
    
    bool CheckCost(CardEffect effect, TriggerData triggerData)
    {
        
        return true;
    }

    #endregion
    

    #region  EffectSys
    
    // 发动一张卡包括 确认是否有cost，如果有支付才能宣言
    async Task AcitveCard(CardEffect currentEffect)
    {
        await PayCost(currentEffect);
        await DeclareEffect(currentEffect);
    }

    async Task PayCost(CardEffect currentEffect)
    {
        
    }
    // 大栈，存放多个小栈（连锁层数）
    Stack<Stack<CardEffect>> _chainStack = new Stack<Stack<CardEffect>>();
    Stack<CardEffect> _temp_chainStack = new Stack<CardEffect>();
    // 效果执行宣言导致的效果检测
    async Task DeclareEffect(CardEffect currentEffect)
    {

        // 3. 如果有响应效果，加入到新的小栈中
        /*_temp_chainStack.Push(currentEffect);*/
    }
    
    int _effectTestCounter = 0; // 用于测试的计数器


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
