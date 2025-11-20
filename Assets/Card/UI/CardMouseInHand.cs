using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
    
public class CardMouseInHand : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    public Vector2 startPos;

    /*private CanvasGroup canvasGroup;*/

    public Card card;
    private void Start()
    {
        /*canvasGroup = GetComponent<CanvasGroup>();*/
        originTransform = transform.localScale;
        startPos = transform.position;
        /*card = this.GetComponent<CardDisplay_InHand>().card;*/

        card=this.GetComponent<HandCardDisplay>().card;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveBack();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        /*Transform grandparentTransform = transform.parent; // 访问父物体的transform
        Transform greatGrandparentTransform = grandparentTransform.parent; // 访问父物体的父物体的transform
        Transform greatGreatGrandparentTransform = greatGrandparentTransform.parent; // 访问父物体的父物体的父物体的transform


        greatGreatGrandparentTransform.SetAsLastSibling();*/
        startPos = transform.position;
        /*canvasGroup.blocksRaycasts = false;*/
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    
    //记得自己回来
    public async void OnEndDrag(PointerEventData eventData)
    {
        if (Vector2.Distance(transform.position, startPos) <= 300)
        {
            // 情况1：拖出不够
            transform.position = startPos;
        }
        else
        {
            WaitingForClickUI();
            bool canUse = await BattleSystem.instance.UseCardCheck(card);
            
            if (!canUse)
            {
                // 情况2：使用失败 → 回到原坐标
                card.sacrificeTargets.Clear();
                card.soulTargets.Clear();
                MoveBack();
                Debug.Log("卡牌使用失败，回到原位置");
            }
            else
            {
                // 情况3：使用成功 → 可能直接销毁/移动卡牌
                // 例如：Destroy(gameObject);
            }
        }
    }

    Vector3 originTransform;

    public void WaitingForClickUI()
    {
        transform.position = new Vector2(200, 800);
        transform.localScale = transform.localScale * 2;
    }

    public void MoveBack()
    {
        transform.localScale = originTransform;
        transform.position = startPos;
    }

}