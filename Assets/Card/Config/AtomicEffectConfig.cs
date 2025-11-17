using System;

[Serializable]
public struct AtomicEffectConfig
{
    public int effectID;          // 原子效果ID
    public string effectName;     // 原子效果名
    public string internalDesc;   // 自用描述 Atomic_Text
    public string effect;         // 原子效果类型 / 名
    public string effectDetail;   // 原子效果参数内容
    public string target;         // 对象条件
    public string effectDuration; // 持续时间
    public string extra;          // 额外信息

    public AtomicEffectConfig(string atomicEffectContent)
    {
        // 先给所有字段一个默认值（struct 构造函数必须全部赋值）
        effectID       = 0;
        effectName     = string.Empty;
        internalDesc   = string.Empty;
        effect         = string.Empty;
        effectDetail   = string.Empty;
        target         = string.Empty;
        effectDuration = string.Empty;
        extra          = string.Empty;

        if (string.IsNullOrWhiteSpace(atomicEffectContent))
            return;

        string[] datas = atomicEffectContent.Split(',');

        if (datas.Length < 13)
            throw new ArgumentException($"AtomicEffect 行列数不足 13，实际 {datas.Length}。数据：{atomicEffectContent}");

        // 去掉首尾空白
        for (int i = 0; i < datas.Length; i++)
            datas[i] = datas[i]?.Trim();

        int.TryParse(datas[0], out effectID); // EffectID
        effectName     = datas[1];            // Effect_Name
        internalDesc   = datas[3];            // Atomic_Text
        effect         = datas[5];            // Effect
        effectDetail   = datas[6];            // Effect_Detail
        target         = datas[9];            // Target
        effectDuration = datas[10];           // EffectDuration
        extra          = datas[12];           // Extra_Info
    }
}