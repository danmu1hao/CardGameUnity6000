using System;
using UnityEngine;

public class Timing
{


    public CardEnums.TimingList timingEnum;

    public bool LoadTiming(string timingText)
    {
        bool success = Enum.TryParse(timingText, out timingEnum);
        if (success)
        {
            Debug.Log($"✅ 成功将 '{timingText}' 解析为 TimingList.{timingEnum}");
        }
        return success;
    }


}