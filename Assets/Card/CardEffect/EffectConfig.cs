using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EffectConfig
{
    public int effectID;           // 效果id efffect_id
    public string effectName;      // 效果名 effect_name
    public string effectText;      // 文本描述 effect_text
    public string internalDesc;    // 自用描述（内部注释/设计备注）
    public string effectType;      // 效果类型 effect_type
    public string effect;          // 效果名/主效果 effect
    public string effectDetail;    // 效果细节 effect_detail
    public string timing;          // 触发时机 timing
    public string condition;       // 条件 condition（CSV里拼写为 codition）
    public string target;          // 作用目标 target
    public string effectDuration;  // 持续时间 effectDuration
    public string cardStyle;       // 卡牌流派 card_style
    public string extra;           // 额外信息 extra_info
    
    /// <summary>
    /// 从一行 CSV（13 列，顺序见上）构建 EffectConfig。
    /// 支持双引号字段与字段内逗号（例："deal, then draw"）。
    /// </summary>
    public EffectConfig(string effectData)
    {
        string[] parts=effectData.Split(',');
        if (parts.Length < 13)
            throw new ArgumentException($"CSV 列数不足：需要 13 列，实际 {parts.Length}。数据：{effectData}");

        // 去除首尾空白
        for (int i = 0; i < parts.Length; i++)
            parts[i] = parts[i]?.Trim();

        // 0: effect_id

        string effectID = parts[0].Substring(0, 4);
        Console.WriteLine(effectID);  // 输出 "1234"
        if (!int.TryParse(effectID, out this.effectID))
        {
            Debug.Log("effectid转换失败");
            this.effectID = 0; // 解析失败给默认值（可按需改成抛异常）
        }


        // 按新表头映射
        effectName      = parts[1];
        effectText      = parts[2];
        internalDesc    = parts[3];
        effectType      = parts[4];
        effect          = parts[5];
        effectDetail    = parts[6];
        timing          = parts[7];
        condition       = parts[8];
        target          = parts[9];
        effectDuration  = parts[10];
        cardStyle       = parts[11];
        extra           = parts[12];
    }


}
