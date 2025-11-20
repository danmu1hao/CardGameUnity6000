using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TargetEnum = CardEnums.ObjectEnum;

public class Target
{
    public AtomicEffect atomicEffect;
    public List<string> targetStringList;
    
    //目前构想 select_count or self,player...
    public  List<Card> targetCards = new List<Card>();
    

    public Player targetPlayer = null;
    
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
        
        
        // 情况2 对象已知
        List<Card> allCards = BattleSystem.instance.allCardsInBattle;
        
        foreach (var targetCondition in targetStringList)
        {
            ResolveTargetList(targetCondition, atomicEffect.triggerData);
        }

        if (targetCards.Count == 0 && targetPlayer == null)
        {
            return false;
           Debug.LogWarning("找对象失败？");
        }

        return true;
    }
    
    // TODO 为了以防万一 如果最后效果没有发动成功，需要初始化
    void ReSetTarget()
    {
        targetCards.Clear();
        targetPlayer = null;
    }

    public bool CheckValidTargetsExist()
    {
        // 情况1 选对象 因为只是判断
        foreach (var infoEnum in atomicEffect.AtomicEffect_Extra_Info)
        {
            if (infoEnum==CardEnums.ExtraInfoEnum.Select)
            {
                // 要选择对象
            }
        }
        
        
        // 情况2 对象已知
        List<Card> allCards = BattleSystem.instance.allCardsInBattle;
        
        foreach (var targetCondition in targetStringList)
        {

            
            ResolveTargetList(targetCondition,  atomicEffect.triggerData);
        }

        if (targetCards.Count == 0 && targetPlayer == null)
        {
            return false;
           Debug.LogWarning("找对象失败？");
        }

        return true;

    }
    
    public bool CheckTarget(Card card)
    {
        return targetCards.Contains(card);
    }
    
    // csharp
    private void ResolveTargetList(string keyword,  TriggerData triggerData)
    {
        Debug.Log("判断对象内容为"+keyword);
        string[] targetSplit = keyword.Split('=');
        // 每次解析前清空上次结果
        targetCards = new List<Card>();
        targetPlayer = null;
        Debug.Log("Target is "+targetSplit[1]);

        TargetEnum targetEnum = CardEnums.TryGetEnum<TargetEnum>(targetSplit[1]);
        
        switch (targetEnum)
        {
            case TargetEnum.Self:
                targetCards = new List<Card> { this.atomicEffect.card };
                break;

            case TargetEnum.Attacker:
                if (triggerData?.Source != null)
                    targetCards = new List<Card> { triggerData.Source };
                else
                    targetCards = new List<Card>();
                break;

            case TargetEnum.Defender:
                if (triggerData?.MutiTarget != null && triggerData.MutiTarget.Count > 0 && triggerData.MutiTarget[0] != null)
                    targetCards = new List<Card> { triggerData.MutiTarget[0] };
                else
                    targetCards = new List<Card>();
                break;

            case TargetEnum.Me:
                targetPlayer = this.atomicEffect.player;
                break;

            default:
                targetCards = new List<Card>();
                break;
        }
        
       Debug.Log("解析目标完成"+targetCards.Count+"targetCarsNum----Player"+targetPlayer?.playerId);
    }


    
}
