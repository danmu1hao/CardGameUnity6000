public static class CardEnums
{
    // 卡牌所在状态
    public enum CardStateEnum
    {
        InDeck,     // 在牌库
        InHand,     // 在手牌
        InField,   // 在场上
        InDamage,   // 在伤害区
        InCemetery,  // 在弃牌区/墓地
        InSoul      // 在灵魂/魂槽
    }

    // 卡牌类型
    public enum CardType
    {
        Monster,    // 怪兽
        Spell       // 魔法
    }
    public enum ClassEnum
    {
        // 这个比较特殊
        Target,
        
        CurrentPlayer,
        Me,
        Op,
        
        Self,

        Defender,
        Attacker
    }
    // 时点 / 触发时机
    public enum TimingList
    {
        when_turn_start, 
        when_effect_declare, 
        when_attack, 
        when_summon
    }

    // 效果类型
    public enum EffectType
    {
        Active,     // 主动（需要手动发动）
        Passive,    // 被动（满足条件自动触发）
        Action      // 行动（反应效果）
    }

    // 效果（动作/结果）
    public enum Effect
    {
        None,
        CancelEffect,   // 取消效果
        TestSkill,      // 打印测试（比如测试专用）
        DrawCard        // 抽卡
        // 未来可以继续扩展：Destroy, Heal, Buff, Debuff ...
    }



}