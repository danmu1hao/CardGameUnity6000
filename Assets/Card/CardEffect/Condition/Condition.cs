using System.Collections.Generic;

public class Condition
{
    public string[] conditionList;
    public CardEffect cardEffect;
    public Condition(string[] condition,CardEffect cardEffect)
    {
        conditionList = condition;
        this.cardEffect = cardEffect;
    }

    public bool CheckConditionAndTarget()
    {
        if (!CheckCondition()) return false;
        if (!CheckTargetExist()) return false;
        return true;
    }

    // TODO 条件确认 对象存在确认
    bool CheckCondition()
    {
        
        // 检查条件是否满足
        foreach (var conditionStr in condition.conditionList)
        {
            Debug.Log(conditionStr);
            //所有条件必须全部满足
            if (!FieldResolver.Resolver(conditionStr,this.card,cardEffect.triggerData))
            {
                
                return false;
            }
        }
        //检查是否有支付cost的条件
    
        return true;
        return true;
    }
    /// <summary>
    /// 很麻烦。。我们居然要同时确认代价对象，效果对象是否存在
    /// </summary>
    /// <returns></returns>
    bool CheckTargetExist()
    {
        
        return true;
    }

    
}