using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public static class FieldResolver
{

    //分析的总接口
    public static bool Resolver(string expression, Card card, TriggerData triggerData)
    {
        // 使用正则识别表达式中的操作符和两边
        var match = System.Text.RegularExpressions.Regex.Match(expression, @"(.+?)\s*(==|=|!=|>|<)\s*(.+)");
        if (!match.Success)
        {
            LogCenter.LogWarning($"无法解析表达式：{expression}");
            return false;
        }

        // 拆分左右部分
        var leftRaw = match.Groups[1].Value.Trim();
        var op = match.Groups[2].Value.Trim();
        var rightRaw = match.Groups[3].Value.Trim();

        // 使用你已有的方法提取左边变量的值
        string leftValue = GetVaribleData(leftRaw, card, triggerData);
        string rightValue = GetVaribleData(rightRaw, card, triggerData);
        if(leftValue=="null"||rightValue=="null"){ LogCenter.LogError("resolve fail");return false;}
        // ( leftValue op rightValue)
        // 重组为简单表达式字符串，如 "5 > 3"
        string evalExpression = $"{leftValue} {op} {rightValue}";

        // 调用你已有的表达式评估器
         LogCenter.Log(leftValue+" "+rightValue+" "+evalExpression+" "+SimpleParser.Evaluate(evalExpression));
        return SimpleParser.Evaluate(evalExpression);
    }
    
    //获取目标变量的总接口 比如 self.atk 获取自身卡牌的攻击力
    public static string GetVaribleData(string keyword, Card card, TriggerData triggerData)
    {
        if (string.IsNullOrEmpty(keyword)) return "";
        /* LogCenter.Log(keyword);*/
        // 判断是否带有属性访问符（.）
        // 只有1个的情况
        var parts = keyword.Split('.');
        if (parts.Length == 1)
        {
             LogCenter.Log("这个不需要解析"+keyword);
            return keyword;
        }
        // 两个 class.varible
        // 三个 class.List.特殊判断
        // TODO 先处理有两个的情况
        if (parts.Length == 2)
        {
            string className = parts[0].Trim().ToLower();   // 如 attacker
            string fieldName = parts[1].Trim();            // 如 hp
            
            IClassResolver classResolver = ResolveClass(className,card,triggerData);
            if (classResolver == null) return null;
            // TODO 注意：在3个以前没必要进行特殊处理，可统一通过找变量返回string
            string value = GetCardFieldValue(classResolver, fieldName);
            if (value == null) return null;
            return value;
        }
        
        // TODO 有三个的情况
        return null;
        // return GetCardFieldValue(cards[0], fieldName);
    }
    //获取对象，比如，将self识别为自身，并且返回对象的Card
    private static IClassResolver ResolveClass(string classType, Card card, TriggerData triggerData)
    {
        IClassResolver result = null;
        
        if (Enum.TryParse("attack", ignoreCase: true, out CardEnums.ObjectEnum type))
        {
             LogCenter.Log($"转换成功: {type}");
        }
        else
        {
             LogCenter.LogWarning("转换失败");
            return null;
        }
        
        switch (type)
        {
            case CardEnums.ObjectEnum.CurrentPlayer:
                result = BattleSystem.instance.currentPlayer;
                break;

            case CardEnums.ObjectEnum.Me:
                result = BattleSystem.instance.GetPlayer(card.playerID,true);
                break;

            case CardEnums.ObjectEnum.Op:
                result = BattleSystem.instance.GetPlayer(card.playerID,false);
                break;

            case CardEnums.ObjectEnum.Self:
                result = card;
                break;
            
            case CardEnums.ObjectEnum.Defender:
                if (triggerData.MutiTarget != null && triggerData.MutiTarget.Count > 0)
                    result = triggerData.MutiTarget[0];
                break;

            case CardEnums.ObjectEnum.Attacker:
                result = triggerData.Source;
                break;
            // case ClassEnum.Target:
            //     result = triggerData.MutiTarget;
            //     break;



            default:
                 LogCenter.LogWarning($"未处理的 ClassEnum: {classType}");
                break;
        }



        return null;
    }
    //获取对象属性，比如，将self.atk识别为自身卡牌的攻击力
    private static string GetCardFieldValue(IClassResolver classResolver, string field)
    {
        if (classResolver == null || string.IsNullOrEmpty(field))
            return "";
        
        // 先找属性
        var prop = classResolver.GetType().GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (prop != null)
        {
            var value = prop.GetValue(classResolver);
            return value?.ToString() ?? "";
        }

        // 再找字段
        var fieldInfo = classResolver.GetType().GetField(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (fieldInfo != null)
        {
            var value = fieldInfo.GetValue(classResolver);
            return value?.ToString() ?? "";
        }

         LogCenter.LogWarning($"字段 '{field}' 不存在于卡牌对象中");
        return "";
    }

}