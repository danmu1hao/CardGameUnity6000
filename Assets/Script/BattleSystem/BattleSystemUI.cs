using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSystemUI : QuickInstance<BattleSystemUI>
{

    #region 变量
    Card currentCard;


    [SerializeField] GameObject unitPrefab;
    [SerializeField] GameObject playerBattleField;
    [SerializeField] GameObject enemyBattleField;

    [SerializeField] GameObject handCardPrefab;
    [SerializeField] GameObject enemyHandCardPrefab;
    [SerializeField] GameObject playerHandCardField;
    [SerializeField] GameObject enemyHandCardField;

    [SerializeField] GameObject playerDamageArea;
    [SerializeField] GameObject enemyDamageArea;

    [SerializeField] GameObject soulCardUIArea;
    
    [SerializeField] GameObject effectCardsUIArea;
    
    #endregion


    public void AllotField()
    {
        for (int i = 0; i < 3; i++)
        {
            //有点草率，先这样
            BattleSystem.instance.Player1.fields[i].fieldModel = playerBattleField.transform.GetChild(i).gameObject;
            BattleSystem.instance.Player1.fields[i].fieldModel.GetComponent<FieldUI>().Init(BattleSystem.instance.Player1.fields[i]);
            BattleSystem.instance.Player2.fields[i].fieldModel = enemyBattleField.transform.GetChild(i).gameObject;
            BattleSystem.instance.Player2.fields[i].fieldModel.GetComponent<FieldUI>().Init(BattleSystem.instance.Player2.fields[i]);
        }
    }


    #region 使用卡牌


    public void UseCard(Card card,GameObject selectSlot=null)
    {
        // 会不会有法术卡？
        DeleteCard(card);
        if (selectSlot!=null)
        {
            GameObject cardModel = Instantiate(unitPrefab, selectSlot.transform);
            cardModel.transform.localPosition = Vector3.zero;
            cardModel.GetComponent<UnitCardDisplay>().Init(card);
            card.cardModel = cardModel;
        }


    }

    #endregion

    #region 手卡UI动画

    /// <summary>
    /// 如果要加多人对战还是需要Player的
    /// </summary>
    public void DrawCard(bool isPlayer, Player player, Card card)
    {
        GameObject cardModel=GameObject.Instantiate(handCardPrefab);
        cardModel.GetComponent<HandCardDisplay>().Init(card);
        if (isPlayer)
        {
            cardModel.transform.SetParent(playerHandCardField.transform);
            StartCoroutine(playerHandCardField.GetComponent<HandCardLayoutUI>().AddCard(cardModel.gameObject.GetComponent<RectTransform>()));
        }
        else
        {
            cardModel.transform.SetParent(enemyHandCardField.transform);
            StartCoroutine(enemyHandCardField.GetComponent<HandCardLayoutUI>().AddCard(cardModel.gameObject.GetComponent<RectTransform>()));
        }
        
        card.cardModel = cardModel;
    }

    // TODO 删卡
    public void DeleteCard( Card card)
    {
        GameObject cardModel = card.cardModel;
        if (card.player==BattleSystem.instance.Player1)
        {
            playerHandCardField.GetComponent<HandCardLayoutUI>().DeleteCard(cardModel.gameObject.GetComponent<RectTransform>());
        }
        else
        {
            enemyHandCardField.GetComponent<HandCardLayoutUI>().DeleteCard(cardModel.gameObject.GetComponent<RectTransform>());
        }
        GameObject.Destroy(cardModel);  
    }

    #endregion


    public void AddCardToDamageArea( List<Card> cards)
    {
        foreach(Card card in cards)
        {
            GameObject cardModel;
            if (card.playerID==0)
            {
                cardModel= Instantiate(handCardPrefab, playerDamageArea.transform);
                
            }
            else
            {
                cardModel= Instantiate(handCardPrefab, enemyDamageArea.transform);
                
            }
            cardModel.GetComponent<HandCardDisplay>().Init(card);
        }
    }
    
    public void GameOver()
    {
        // 结束战斗处理
    }

    #region 工具方法

    public static void DestoryModel(GameObject obj)
    {
        Destroy(obj);
    }

    #endregion

}
