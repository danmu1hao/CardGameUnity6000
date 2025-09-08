using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EffectCardsPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(ActiveSelectedCard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private TaskCompletionSource<bool> _tcs;
    public GameObject effectCardPrefab;
    public Transform EffectCardsPanelUI;
    public void ShowEffectCards(List<Card> cardList, TaskCompletionSource<bool> tcs)
    {
        this._tcs = tcs;
        effectModelList.Clear();
        foreach (var card in cardList)
        {
            GameObject cardModel = Instantiate(effectCardPrefab, EffectCardsPanelUI);
            cardModel.GetComponent<EffectCardDisplay>().Init(card,this);
            effectModelList.Add(cardModel);
        }
    }

    public List<GameObject> effectModelList;

    private EffectCardDisplay currentSelected = null;
    public void SelectCard(EffectCardDisplay selected)
    {
        if (currentSelected != null && currentSelected != selected)
        {
            currentSelected.SetActiveState(false); // 关闭旧的
        }

        if (currentSelected == selected)
        {
            // 如果是同一张卡，再次点击 → 关闭并清空
            currentSelected.SetActiveState(false);
            currentSelected = null;
        }
        else
        {
            selected.SetActiveState(true);
            currentSelected = selected;
        }
    }

    public Button button;
    public void ActiveSelectedCard()
    {
        if (currentSelected != null)
        {
            Debug.Log($"[EffectPanel] 玩家选择发动卡牌：{currentSelected.card.name}，共 {currentSelected.card.canEffectList.Count} 个效果");

            foreach (var cardEffect in currentSelected.card.canEffectList)
            {
                Debug.Log($"[EffectPanel] 加入效果队列：{cardEffect.effectConfig.effectText}");
                EffectSystem.Instance.EnQueue(cardEffect);
            }
        }
        else
        {
            Debug.Log("[EffectPanel] 没有选中的卡牌，跳过发动");
        }

        effectModelList.Remove(currentSelected.GameObject());
        Debug.Log($"[EffectPanel] 当前剩余待选择卡数：{effectModelList.Count}");

        if (effectModelList.Count == 0)
        {
            Debug.Log("[EffectPanel] 所有效果卡已处理，关闭面板");
            ClosePanel();
        }
    }


    public void ClosePanel()
    {
        _tcs?.SetResult(true);
        gameObject.SetActive(false);

        
    }
}
