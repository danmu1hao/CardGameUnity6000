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

    // 代价类型
    public enum CostType
    {
        None,
        Sacrifice,  // 献祭
        Discard     // 丢弃
        // 以后可以加：PayLife, RemoveSoul 等
    }
    

    // 对象限制（谁是效果目标）
    public enum Target
    {
        None,
        Self,       // 自身
        Opponent,   // 对手
        CardType,   // 特定卡牌类型
        CardState   // 特定卡牌状态
    }
}