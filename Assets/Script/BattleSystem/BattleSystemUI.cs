using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSystemUI : SimpleSingleton<BattleSystemUI>
{
    public static BattleSystemUI instance { get; private set; }

    bool chooseSlot;
    Card currentCard;
    GameObject selectSlot;

    [SerializeField] GameObject unitPrefab;
    [SerializeField] GameObject playerBattleField;
    [SerializeField] GameObject enemyBattleField;

    [SerializeField] GameObject handCardPrefab;
    [SerializeField] GameObject enemyHandCardPrefab;
    [SerializeField] GameObject playerHandCardField;
    [SerializeField] GameObject enemyHandCardField;

    [SerializeField] TextMeshProUGUI playerCurrentMana;
    [SerializeField] TextMeshProUGUI enemyCurrentMana;

    [SerializeField] GameObject playerDamageArea;
    [SerializeField] GameObject enemyDamageArea;

    [SerializeField] GameObject soulCardUIArea;
    
    [SerializeField] GameObject effectCardsUIArea;


    void Start()
    {

        AllotField();
    }

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
    
    void Update()
    {
        if (chooseSlot && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log($"Hit UI element: {hit.collider.gameObject.name}");
                chooseSlot = false;
                selectSlot = hit.collider.gameObject;
                BattleSystem.instance.UseCard(currentCard.player, currentCard,selectSlot);
            }
        }
    }

    public GameObject selectPanel;
    public void ShowSelectPanel()
    {

    }


    public void DestroyCard(Card card)
    {
        Destroy(card.cardModel);
    }

    #region 使用卡牌

    public void UseCardCheck(Player player, Card card)
    {
        card.cardModel.GetComponent<CardMouseInHand>().WaitingForClickUI();
        currentCard = card;
        chooseSlot = true;
    }

    public void UseCard(Player player, Card card)
    {
        Destroy(card.cardModel);
        GameObject cardModel = Instantiate(unitPrefab, selectSlot.transform);
        cardModel.transform.localPosition = Vector3.zero;
        cardModel.GetComponent<UnitCardDisplay>().Init(card);
        card.cardModel = cardModel;
    }

    #endregion
     public void DrawCard(bool isPlayer, Player player, Card card)
    {

        GameObject cardModel;
        if (isPlayer)
        {
            cardModel = Instantiate(handCardPrefab, playerHandCardField.transform);
        }
        else
        {
            cardModel = Instantiate(enemyHandCardPrefab, enemyHandCardField.transform);
        }
        cardModel.GetComponent<HandCardDisplay>().Init(card);
        card.cardModel = cardModel;
    }



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
    
    public  void DestoryModel(GameObject obj)
    {
        Destroy(obj);
    }
}
