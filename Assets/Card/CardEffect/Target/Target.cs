using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Target
{
    /*public Target(string[] target,CardEffect effect)
    {
        this.effect = effect;
        this.target = target;
    }

    CardEffect effect;
    string[] target;*/
    
    //目前构想 select_count or self,player...
    public static List<ITargetable> FindTarget(string keyword, Card card, TriggerData triggerData)
    {
        Debug.Log($"[FindTarget] 收到目标字符串：{keyword}");

        string[] targetList = keyword.Split('=');
        if (targetList.Length < 2)
        {
            Debug.LogError($"[FindTarget] 目标格式错误：{keyword}");
            return new List<ITargetable>();
        }

        string targetKeyword = targetList[1];
        Debug.Log($"[FindTarget] 解析目标字段为：{targetKeyword}");

        var result = ResolveTargetList(targetKeyword, card, triggerData);

        Debug.Log($"[FindTarget] 成功解析目标，返回 {result?.Count ?? 0} 个对象");
        return result;
    }

    //赋值操作，这里暂且再来一次
    private static List<ITargetable> ResolveTargetList(string keyword, Card card, TriggerData triggerData)
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
