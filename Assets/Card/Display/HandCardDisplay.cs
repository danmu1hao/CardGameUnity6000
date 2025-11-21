using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HandCardDisplay : MonoBehaviour
{
    private int handIndexUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Card card;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardSoulCostText;
    public TextMeshProUGUI cardSacrifiseCostText;
    public TextMeshProUGUI cardAtk;
    public Image cardImage;

    public GameObject soulUI;
    public GameObject sacrificeUI;
    public void Init(Card card)
    {
        this.card = card;
        cardName.text = card.name;
        
        if ( card.soulCost!=-1)
        {
            soulUI.SetActive(true);
            cardSoulCostText.text = card.soulCost.ToString();
        }else if (card.sacrificeCost != -1)
        {
            sacrificeUI.SetActive(true);
            cardSacrifiseCostText.text = card.sacrificeCost.ToString();
        }


        cardSoulCostText.text = card.soulCost.ToString();
        cardSacrifiseCostText.text = card.sacrificeCost.ToString();

        cardAtk.text = card.atk.ToString();


        if (card.cardImage != null)
        {
            // 找到了图片，使用它
            cardImage.sprite = card.cardImage;
        }
        /*else
        {
            // 没找到，什么也不做（或打印提示）
            Debug.LogWarning($"未找到卡牌图片：CardImage/{card.id}");
        }*/
        
    }
}
