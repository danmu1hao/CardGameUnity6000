using UnityEngine;
using System.Collections.Generic;

public class FieldResolverTester : MonoBehaviour
{
    void Start()
    {
        // 构造测试卡牌
        var card = new Card();
        card.CardConfig = new CardConfig();
        card.CardConfig.type = "abc";
        card.atk = 5;
        card.name = "TestCard";

        var triggerData = new TriggerData(card, new List<Card> { card });

        // 测试案例列表
        var testCases = new List<string>
        {
            "self.atk > 3",           // int > int（true）
            "self.atk < 3",           // int < int（false）
            "self.name = TestCard",   // string = string（true）
            "self.name = WrongName",  // string = string（false）
            "self.type = abc" // nested object field（true）
        };

        foreach (var expr in testCases)
        {
            bool result = FieldResolver.Resolver(expr, card, triggerData);
            Debug.Log($"表达式：{expr} → 结果：{result}");
        }
    }
}