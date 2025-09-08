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
    public void OnEndDrag(PointerEventData eventData)
    {
        //情况1 拖出区域不够，没使用 情况2 拖出区域够了，使用了，移到左上角，但是最后发现还是等于没使用，回到原坐标
        //情况3 拖出区域够了，使用了，卡牌被消灭了，看不到所以问题不大
        if (Vector2.Distance(transform.position, startPos) <= 300)
        {
            transform.position = startPos;
        }
        else
        {
            transform.position = startPos;
            if (card.player == null)
            {
                Debug.LogWarning("woca");
            }
            BattleSystem.Instance.UseCardCheck(card.player,card);
        }

        /*canvasGroup.blocksRaycasts = true;*/
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