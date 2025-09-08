using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseClickHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arror;
    GameObject arrorObj;
    // 当拖拽开始时调用
    public Card card;
    public void Start()
    {
        card = this.GetComponent<UnitCardDisplay>().card;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CanAttack())
        {

            arrorObj = Instantiate(arror, transform);
            Vector2 position = arrorObj.transform.position;
            Vector2 exactpostion = transform.position;
            Debug.Log(exactpostion.x + "OnBeginDrag" + exactpostion.y);
            arrorObj.GetComponent<Arrow>().offsetVector = exactpostion;

            arrorObj.SetActive(true);

            /*BattleSystem.Instance.Instance.attackerCard = gameObject.GetComponent<CardDisplay_InPlay>().card; */
        }
    }

    // 在拖拽过程中持续调用
    public void OnDrag(PointerEventData eventData)
    {

    }

    // 当拖拽结束时调用
    public void OnEndDrag(PointerEventData eventData)
    {
        if (CanAttack())
        {
            Destroy(arrorObj);
            Debug.Log("OnEndDrag");
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject != null)
                {
                    Card target = null;
                    if (hit.collider.gameObject.GetComponent<UnitCardDisplay>() != null)
                    {
                        target = hit.collider.gameObject.GetComponent<UnitCardDisplay>().card;
                        Debug.Log(target.name);
                        BattleSystem.Instance.BattleConfirm(card, target);
                    }
                    
                }
            }
        }
    }
    public bool CanAttack()
    {
        /*Card card = gameObject.GetComponent<CardDisplay_InPlay>().card;
        if (card.maxAtkTimes - card.currentAtkTimes > 0&&card.canAtk_SummonTurn== true)
        {
            return true;
        }
        else { return false; }*/
        return true;
    }
}
