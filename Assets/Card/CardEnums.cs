using System;
using UnityEngine;

public static class CardEnums
{
    // 卡牌所在状态
    public enum CardStateEnum
    {
        None,
        InDeck,     // 在牌库
        InHand,     // 在手牌
        InField,   // 在场上
        InDamage,   // 在伤害区
        InCemetery,  // 在弃牌区/墓地
        InSoul      // 在灵魂/魂槽
    }

    // 卡牌类型
    public enum CardTypeEnum
    {
        None,
        Monster,    // 怪兽
        Spell       // 魔法
    }
    public enum ObjectEnum
    {
        None,
        // 这个比较特殊,指代效果对象
        Target,
        
        CurrentPlayer,
        Me,
        Op,
        
        Self,

        Defender,
        Attacker
    }
    // 时点 / 触发时机
    public enum TimingEnum
    {
        None,
        when_turn_start, 
        when_effect_declare, 
        when_attack, 
        when_summon
    }

    public enum ExtraInfoEnum
    {
        None,
        Pre,
        Select
    }

    // 效果类型
    public enum EffectTypeEnum
    {
        None,
        Active,     // 主动（需要手动发动）
        Response,    // 被动（满足条件自动触发）
        Trigger,      // 行动（反应效果）
        Continuous
    }

    // 效果（动作/结果）
    public enum EffectEnum
    {
        None,
        CancelEffect,   // 取消效果
        TestSkill,      // 打印测试（比如测试专用）
        DrawCard        // 抽卡
        // 未来可以继续扩展：Destroy, Heal, Buff, Debuff ...
    }


    /// <summary>
    /// 这个方法可行
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T TryGetEnum<T>(string value) where T : struct, Enum
    {
        if (Enum.TryParse<T>(value, true, out var result)) return result;
        Debug.LogWarning("enum转换失败");   
        if (string.IsNullOrWhiteSpace(value)) return default;
        return default;
    }


}