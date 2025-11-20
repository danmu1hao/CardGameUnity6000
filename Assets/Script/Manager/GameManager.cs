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
    [SerializeField] TextAsset enemyDeck;
    
    /// <summary>
    /// cardid->CardConfig 
    /// </summary>
    public Dictionary<int, CardConfig> cardConfigDict = new Dictionary<int, CardConfig>();
    

    /// <summary>
    /// cardid -> List &lt;EffectConfig&gt; 卡牌id可以获取自己的多个Effect
    /// </summary>
    public Dictionary<int,List<CardEffectConfig>> cardID_EffectConfigDict = new Dictionary<int, List<CardEffectConfig>>();

    /// <summary>
    /// effectID -> &lt;EffectConfig&gt; 卡牌id可以获取自己的多个Effect
    /// </summary>
    public Dictionary<int,CardEffectConfig> effectID_EffectConfigDict = new Dictionary<int, CardEffectConfig>();

    public void Start()
    {
        ReadEffectData();
        ReadCardData();

        
        BattleSystem.instance.BattleStart();

    }

    [SerializeField]TextAsset cardcsvFile;
    [SerializeField]TextAsset effectcsvFile;
    public List<string> cardData;
    public void ReadCardData()
    {
        string textFile = cardcsvFile.text;
        var lines = textFile.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 从第1行开始，跳过第0行
        {
            string line = lines[i];
            var fields = line.Split(',');
            if (string.IsNullOrWhiteSpace(line) || fields.All(string.IsNullOrWhiteSpace))
            {
                continue;
            }
           Debug.Log(line);
            cardData.Add(line);
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
            var effectData = line.Split(',');
            if (string.IsNullOrWhiteSpace(line) || effectData.All(string.IsNullOrWhiteSpace))
            {
                continue;
            }

            string effectIDStr = effectData[0].Trim();
            if (effectIDStr.Length < 9 || effectIDStr.Length > 10)
            {
               Debug.LogWarning($"EffectID 长度不合理 ：{effectIDStr}（行:{line})");
                continue;
            }

            
            // 拆 EffectID：卡牌ID(0..7)、效果序号(8)、原子序号(9)
            string cardIDStr = effectIDStr.Substring(0, 8); // 8 位卡牌ID
            int.TryParse(cardIDStr,out int cardID);
            string cardEffectIDStr = effectIDStr.Substring(0, 9);
            int.TryParse(cardEffectIDStr,out int effectID);
            if (effectIDStr.Length == 9)
            {
                // 9位 CardEffect
                CardEffectConfig cardEffectConfig = new CardEffectConfig(line);
                if (!cardID_EffectConfigDict.ContainsKey(cardID))
                    cardID_EffectConfigDict.Add( cardID ,new List<CardEffectConfig>(){cardEffectConfig});
                else{cardID_EffectConfigDict[cardID].Add(cardEffectConfig);}
                
                effectID_EffectConfigDict.Add(effectID,cardEffectConfig);
            }else if (effectIDStr.Length ==10 )
            {
                // 10位 AtomicEffect
                if (!effectID_EffectConfigDict.ContainsKey(effectID))
                {
                    Debug.LogWarning("出现了没有父效果的原子效果");
                }
                else
                {
                    effectID_EffectConfigDict[effectID].AddAtomicEffectConfig(line);
                }
            }


        }
        
    }

    /*
    Debug.Log(cardConfigDict.Count);
    foreach (var VARIABLE in cardConfigDict)
    {
        Debug.LogLogWarning(VARIABLE.Key);
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
