using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCardDisplay : MonoBehaviour, IPointerClickHandler
{
    public GameObject selecetUI; // 👈 要切换显示/隐藏的目标

    public bool isActive = false;

    private void Start()
    {
        selecetUI.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _selectPanelManager.SelectCard(this);
    }
    //没注释就是头疼，我这个代码的意义是一次只能选一张卡方便我做判断
    public void SetActiveState(bool value)
    {
        isActive = value;
        selecetUI.SetActive(isActive);
    }
    
    public Card card;
    [SerializeField]TextMeshProUGUI cardName;

    [SerializeField]TextMeshProUGUI cardAtk;
    [SerializeField]Image cardImage;

    SelectPanelManager _selectPanelManager;
    public void Init(Card card,SelectPanelManager selectPanelManager)
    {
        this.card = card;

        this._selectPanelManager = selectPanelManager;

        cardName.text = card.name;

        cardAtk.text = card.atk.ToString();
    }

}
