using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : QuickInstance<GameManager>
{
    public TextAsset playerDeck;
    public TextAsset enemyDeck;
    public Dictionary<int, CardConfig> cardConfigDict = new Dictionary<int, CardConfig>();

    public void Start()
    {
        ReadCardData();
        BattleSystem.instance.BattleStart();

    }

    public TextAsset excelFile;
    public string[] cardData;
    public void ReadCardData()
    {
        string textFile = excelFile.text;
        cardData = textFile.Split('\n');

        Debug.Log(cardData);

        for (int i = 1; i < cardData.Length; i++) // 从第1行开始，跳过第0行
        {
            string line = cardData[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            Debug.Log(line);

            CardConfig card = new CardConfig(line);
            cardConfigDict.Add(card.id, card);
        }
    }

    /*
    Debug.Log(cardConfigDict.Count);
    foreach (var VARIABLE in cardConfigDict)
    {
        Debug.LogWarning(VARIABLE.Key);
    }*/
    public void PrepareDeck()
    {
        PrepareDeck(BattleSystem.instance.Player1, playerDeck);
        PrepareDeck(BattleSystem.instance.Player2, enemyDeck);
    }

    public void PrepareDeck(Player player, TextAsset deck)
    {
        string data = deck.text;
        // string[] cardDatas = data.Split('\n');
        string[] cardDatas = deck.text
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var cardData in cardDatas)
        {
            string[] cardCount = cardData.Split(',');
            Debug.Log(cardCount.Length);
            Debug.Log(cardCount[0]);
            // 调试：把行内每个字符的十六进制码位打印出来
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (char ch in cardData)
            {
                sb.AppendFormat("\\u{0:X4}", (int)ch);  // 例如 \uFEFF \u000D \u000A
            }
            Debug.Log($"RAW LINE HEX: {sb}");  // 如果第一条打印出 \uFEFF\u000D 之类，就对了

            
            int id = int.Parse(cardCount[0]);
            Debug.Log(id);
            int cardNum = int.Parse(cardCount[1]);

            for (int i = 0; i < cardNum; i++)
            {
                Card card = new Card(player, cardConfigDict[id]);

                card.Move(CardEnums.CardStateEnum.InDeck);

            }
            Debug.Log(player.inDeckCards.Count);

        }
        ListExtensions.Shuffle(player.inDeckCards);
    }

    public CardConfig GetCardConfigById(int id)
    {
        if (cardConfigDict.TryGetValue(id, out var config))
        {
            return config;
        }
        Debug.LogError($"CardConfig with ID {id} not found.");
        return default;
    }

}
