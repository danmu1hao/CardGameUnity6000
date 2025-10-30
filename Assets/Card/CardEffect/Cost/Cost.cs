using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost
{
    public CardEffect cardEffect;
    string[] cost;
    public Cost(string[] cost, CardEffect cardEffect)
    {
        this.cost = cost;
        this.cardEffect = cardEffect;
    }
    //通常分为target和effect
    public void PayCost(string cost)
    {
        Debug.Log($"[PayCost] 开始处理代价字符串：{cost}");

        List<ITargetable> targets = new List<ITargetable>();

        foreach (var str in this.cost)
        {
            Debug.Log($"[PayCost] 处理目标语句：{str}");

            if (str.StartsWith("target="))
            {
                Debug.Log("[PayCost] 识别为目标指令，调用 Target.FindTarget");

                targets=Target.FindTarget(str, cardEffect.card, cardEffect.triggerData);

                Debug.Log("[PayCost] 已调用 Target.FindTarget，但尚未将结果加入 targets（注意）");
            }
        }

        foreach (var str in this.cost)
        {
            Debug.Log($"[PayCost] 处理效果语句：{str}");

            if (str.StartsWith("effect="))
            {
                Debug.Log("[PayCost] 识别为效果指令，准备查找效果类");
                Debug.Log("[PayCost] 识别为效果指令，准备查找效果类");

                string[] effectStr = str.Split('=');

                Debug.Log($"[PayCost] 解析到效果类名：{effectStr[1]}");

                Effect effect = ReflactionSystem.FindClassByName<Effect>(effectStr[1]);

                if (effect == null)
                {
                    Debug.LogError($"[PayCost] 找不到效果类：{effectStr[1]}");
                }

                if (targets.Count == 0)
                {
                    Debug.LogWarning("[PayCost] 没有找到任何目标，效果可能无效");
                }

                Debug.Log($"[PayCost] 开始对 {targets.Count} 个目标执行效果：{effect.GetType().Name}");
                effect.EffectExecute();
                Debug.Log("[PayCost] 效果执行完成");
            }
        }

        Debug.Log("[PayCost] 代价处理完成");
    }

}
