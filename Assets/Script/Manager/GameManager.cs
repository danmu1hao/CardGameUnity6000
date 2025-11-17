using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : QuickInstance<GameManager>
{
    [SerializeField]TextAsset playerDeck;
    TextAsset enemyDeck;
    
    /// <summary>
    /// cardid->CardConfig 
    /// </summary>
    public Dictionary<int, CardConfig> cardConfigDict = new Dictionary<int, CardConfig>();
    
    /// <summary>
    /// cardid->自己的所有的effectID
    /// </summary>
    public Dictionary<int, List<int>> cardIDEffectConfigDict = new Dictionary<int, List<int>>();
    /// <summary>
    /// effectID->EffectConfig 卡牌id可以获取自己的多个Effect
    /// </summary>
    public Dictionary<int, CardEffectConfig> effectIDEffectConfigDict = new Dictionary<int,CardEffectConfig>();
    public void Start()
    {
        ReadCardData();
        ReadEffectData();
        
        BattleSystem.instance.BattleStart();

    }

    TextAsset cardcsvFile;
    TextAsset effectcsvFile;
    public string[] cardData;
    public void ReadCardData()
    {
        string textFile = cardcsvFile.text;
        cardData = textFile.Split('\n');

        for (int i = 1; i < cardData.Length; i++) // 从第1行开始，跳过第0行
        {
            string line = cardData[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            Debug.Log(line);

            CardConfig card = new CardConfig(line);
            cardConfigDict.Add(card.id, card);
        }
    }

    /// <summary>
    /// effect id格式暂定为 xxxx卡牌id + 第x个效果 + 第x个具体效果
    /// 可以根据卡牌id 找到所有卡牌关联效果，然后创造效果类
    /// </summary>
    public void ReadEffectData()
    {
        string textFile = effectcsvFile.text;
        string[] effectDataList = textFile.Split('\n');
        
        // 处理单行 data
        // 从第1行开始，跳过表头
        for (int i = 1; i < effectDataList.Length; i++)
        {
            string line = effectDataList[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;             
            string[] effectData = line.Split(',');
            if (effectData.Length == 0)
                continue;

            string effectIDStr = effectData[0].Trim();
            if (effectIDStr.Length < 9)
            {
                LogCenter.Warning($"EffectID 长度不足 9 位：{effectIDStr}（行:{line})");
                continue;
            }

            // 拆 EffectID：卡牌ID(0..7)、效果序号(8)、原子序号(9)
            string cardIDStr = effectIDStr.Substring(0, 8); // 8 位卡牌ID
            string effectIndexStr = effectIDStr.Substring(8, 1); // 第几个效果
            int cardID = 0;
            int effectGroupID = 0;
            if(!int.TryParse(cardIDStr, out  cardID)
               || !int.TryParse(effectIndexStr, out  effectGroupID))
                LogCenter.Warning("读取效果数据失败");

            // 1）填充 cardIDEffectConfigDict：卡牌ID -> 该卡所有效果ID
            if (!cardIDEffectConfigDict.TryGetValue(cardID, out var effectIDList))
            {
                effectIDList = new List<int>(){effectGroupID};
                cardIDEffectConfigDict.Add(cardID, effectIDList);
            }else cardIDEffectConfigDict[cardID].Add(effectGroupID);

            if (!effectIDEffectConfigDict.TryGetValue(effectGroupID, out CardEffectConfig effectConfigList))
            {
                //如果还没有这个Config
                if(effectGroupID.ToString().Length!=9) LogCenter.Warning("效果ID长度错误");
                CardEffectConfig cardEffectConfig = new CardEffectConfig(line);
                effectIDEffectConfigDict.Add(effectGroupID, cardEffectConfig);
            }else
            {
                if(effectGroupID.ToString().Length!=10) LogCenter.Warning("原子ID长度错误");
                effectIDEffectConfigDict[effectGroupID].AddAtomicEffectConfig(line);
            }
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
