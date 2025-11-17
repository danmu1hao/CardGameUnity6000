using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Target
{
    public AtomicEffect atomicEffect;
    public List<string> targetStringList;
    
    //目前构想 select_count or self,player...
    public  List<Card> targetCards = new List<Card>();
    public Player targetPlayer;
    
    public Target(AtomicEffect atomicEffect,List<string> targetStringList)
    {
        this.atomicEffect = atomicEffect;
        this.targetStringList = targetStringList;
    }

    /// <summary>
    /// 有两种选对象方式，赋值，限定
    /// </summary>
    /// <returns></returns>
    public async Task<bool> GetValidTargets()
    {
        List<Card> targets = new List<Card>();
        Debug.Log($"开始查找效果目标");

        // 情况1 选对象 通过额外信息的select_count=1判断
        foreach (var infoEnum in atomicEffect.AtomicEffect_Extra_Info)
        {
            if (infoEnum==CardEnums.ExtraInfoEnum.Select)
            {
                // 要选择对象
            }
        }
        
        
        List<Card> allCards = BattleSystem.instance.allCardsInBattle;
        
        foreach (var targetCondition in targetStringList)
        {
            var result = ResolveTargetList(targetCondition, cardEffect.card, cardEffect.triggerData);
        }
        


        string targetKeyword = targetList[1];
        Debug.Log($"[FindTarget] 解析目标字段为：{targetKeyword}");

        

        Debug.Log($"[FindTarget] 成功解析目标，返回 {result?.Count ?? 0} 个对象");
        return targets;
    }
    
    // public void FindTarget(string keyword, Card card, TriggerData triggerData)
    // {
    //     Debug.Log($"[FindTarget] 收到目标字符串：{keyword}");
    //
    //     string[] targetList = keyword.Split('=');
    //     if (targetList.Length < 2)
    //     {
    //         Debug.LogError($"[FindTarget] 目标格式错误：{keyword}");
    //
    //     }
    //
    //     string targetKeyword = targetList[1];
    //     Debug.Log($"[FindTarget] 解析目标字段为：{targetKeyword}");
    //
    //     var result = ResolveTargetList(targetKeyword, card, triggerData);
    //
    //     Debug.Log($"[FindTarget] 成功解析目标，返回 {result?.Count ?? 0} 个对象");
    //
    // }

    //赋值操作，这里暂且再来一次

    public void CheckValidTargets()
    {
        List<Card> targets = new List<Card>();
        Debug.Log($"开始确认是否存在效果目标");

        List<Card> allCards = BattleSystem.instance.allCardsInBattle;

    }
    
    public bool CheckTarget(Card card)
    {
        return targetCards.Contains(card);
    }
    
    private static void ResolveTargetList(string keyword, Card card, TriggerData triggerData)
    {
        switch (keyword)
        {
            case "self":
                return new List<ITargetable> { card };

            case "attacker":
                return triggerData.Source != null
                    ? new List<ITargetable> { triggerData.Source }
                    : new List<ITargetable>();

            case "defender":
                return (triggerData.MutiTarget != null && triggerData.MutiTarget.Count > 0)
                    ? new List<ITargetable> { triggerData.MutiTarget[0] }
                    : new List<ITargetable>();

            case "player":
                return new List<ITargetable> { card.player };

            default:
                return new List<ITargetable>();
        }
    }

    
}
