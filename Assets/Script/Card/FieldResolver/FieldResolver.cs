using System;
using System.Collections.Generic;
using System.Reflection;
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
            UnityEngine.Debug.LogWarning($"无法解析表达式：{expression}");
            return false;
        }

        // 拆分左右部分
        var leftRaw = match.Groups[1].Value.Trim();
        var op = match.Groups[2].Value.Trim();
        var rightRaw = match.Groups[3].Value.Trim();

        // 使用你已有的方法提取左边变量的值
        string leftValue = GetVaribleData(leftRaw, card, triggerData);
        string rightValue = GetVaribleData(rightRaw, card, triggerData);
        // 重组为简单表达式字符串，如 "5 > 3"
        string evalExpression = $"{leftValue} {op} {rightValue}";

        // 调用你已有的表达式评估器
        Debug.Log(leftValue+" "+rightValue+" "+evalExpression+" "+SimpleParser.Evaluate(evalExpression));
        return SimpleParser.Evaluate(evalExpression);
    }
    
    //获取目标变量的总接口 比如 self.atk 获取自身卡牌的攻击力
    public static string GetVaribleData(string keyword, Card card, TriggerData triggerData)
    {
        if (string.IsNullOrEmpty(keyword)) return "";
        /*Debug.Log(keyword);*/
        // 判断是否带有属性访问符（.）
        var parts = keyword.Split('.');
        if (parts.Length == 1)
        {
            Debug.Log("这个不需要解析"+keyword);
            return keyword;
        }

        string cardName = parts[0].Trim().ToLower();   // 如 attacker
        string fieldName = parts[1].Trim();            // 如 hp

        List<Card> cards = ResolveCardList(cardName, card, triggerData);
        if (cards == null || cards.Count == 0)
        {
            Debug.LogWarning($"未解析到目标卡牌：{cardName}");
            return "";
        }

        return GetCardFieldValue(cards[0], fieldName);
    }
    //获取对象，比如，将self识别为自身，并且返回对象的Card
    private static List<Card> ResolveCardList(string keyword, Card card, TriggerData triggerData)
    {
        switch (keyword)
        {
            case "self":
                return new List<Card> { card };

            case "attacker":
                return triggerData.Source != null ? new List<Card> { triggerData.Source } : new List<Card>();

            case "defender":
                return (triggerData.MutiTarget != null && triggerData.MutiTarget.Count > 0)
                    ? new List<Card> { triggerData.MutiTarget[0] }
                    : new List<Card>();

            case "target":
                return triggerData.MutiTarget ?? new List<Card>();

            default:
                return null;
        }
    }
    //获取对象属性，比如，将self.atk识别为自身卡牌的攻击力
    private static string GetCardFieldValue(Card card, string field)
    {
        if (card == null || string.IsNullOrEmpty(field))
            return "";

        // 先找属性
        var prop = card.GetType().GetProperty(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (prop != null)
        {
            var value = prop.GetValue(card);
            return value?.ToString() ?? "";
        }

        // 再找字段
        var fieldInfo = card.GetType().GetField(field, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (fieldInfo != null)
        {
            var value = fieldInfo.GetValue(card);
            return value?.ToString() ?? "";
        }

        Debug.LogWarning($"字段 '{field}' 不存在于卡牌对象中");
        return "";
    }

}