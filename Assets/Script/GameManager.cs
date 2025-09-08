using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class GameManager : SimpleSingleton<GameManager>
{
    public TextAsset playerDeck;
    public TextAsset enemyDeck;
    public Dictionary<int, CardConfig> cardConfigDict = new Dictionary<int, CardConfig>();

    public static GameManager instance;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // BattleSystem.Instance.BattleStart();
        ReadCardData();
        PrepareDeck(BattleSystem.instance.Player1, playerDeck);
        PrepareDeck(BattleSystem.instance.Player2, enemyDeck);
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

    public void PrepareDeck(Player player, TextAsset deck)
    {
        string data = deck.text;
        string[] cardDatas = data.Split('\n');
        foreach (var cardData in cardDatas)
        {
            string[] cardCount = cardData.Split(',');
            Debug.Log(cardCount.Length);
            int id = int.Parse(cardCount[0]);
            Debug.Log(id);
            int cardNum = int.Parse(cardCount[1]);

            for (int i = 0; i < cardNum; i++)
            {
                Card card = new Card(player, cardConfigDict[id]);

                card.Move(Card.CardStateEnum.InDeck);

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
