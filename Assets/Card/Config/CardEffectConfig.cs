using System;
using System.Collections.Generic;

[Serializable]
public struct CardEffectConfig
{
    // 由父效果管理的原子效果列表
    public List<AtomicEffectConfig> atomicEffectConfigList;
    // public List<int> atomicEffectIDList=>atomicEffectConfigList.ConvertAll(item=>item.effectID);
    //
    // 父效果本身只保留你列出的这些字段
    public int    effectID;      // 效果ID (cardID + n + 0)
    public string effectName;    // 效果名
    public string effectText;    // 效果描述（给玩家看的文本）
    public string effectType;    // 效果类型
    public string timing;        // 时点 (Timing)
    public string condition;     // 条件 (Condition)
    public string cardStyle;     // 卡牌流派 (Card_Style)

    /// <summary>
    /// effectDataList[0] = 父效果那行
    /// effectDataList[1..] = 该父效果的所有原子效果行
    /// </summary>
    public CardEffectConfig(string effectDataList)
    {
        atomicEffectConfigList = new List<AtomicEffectConfig>();

        effectID   = 0;
        effectName = string.Empty;
        effectText = string.Empty;
        effectType = string.Empty;
        timing     = string.Empty;
        condition  = string.Empty;
        cardStyle  = string.Empty;

        string[] cols = effectDataList.Split(',');

        if (cols.Length < 13)
            throw new ArgumentException($"CardEffect 行列数不足 13，实际 {cols.Length}。数据：{effectDataList}");

        for (int i = 0; i < cols.Length; i++)
            cols[i] = cols[i]?.Trim();

        int.TryParse(cols[0], out effectID); // EffectID
        effectName = cols[1];                // Effect_Name
        effectText = cols[2];                // Effect_Text
        effectType = cols[4];                // EffectType
        timing     = cols[7];                // Timing
        condition  = cols[8];                // Condition
        cardStyle  = cols[11];               // Card_Style
        // 其它列（Atomic_Text, Effect, Effect_Detail, Target, EffectDuration, Extra_Info）
        // 交给 AtomicEffectConfig 去处理


    }
    public void AddAtomicEffectConfig(string atomicEffectConfigData)
    {
        AtomicEffectConfig atomicEffectConfig = new AtomicEffectConfig(atomicEffectConfigData);
        atomicEffectConfigList.Add(atomicEffectConfig);
    }
}
