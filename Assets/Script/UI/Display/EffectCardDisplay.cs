using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EffectCardDisplay : MonoBehaviour, IPointerClickHandler
{
    public GameObject selecetUI; // 👈 要切换显示/隐藏的目标

    public bool isActive = false;

    private void Start()
    {
        selecetUI.SetActive(isActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        effectCardsPanel.SelectCard(this);
    }

    public void SetActiveState(bool value)
    {
        isActive = value;
        selecetUI.SetActive(isActive);
    }
    
    public Card card;
    [SerializeField]TextMeshProUGUI cardName;

    [SerializeField]TextMeshProUGUI cardAtk;
    [SerializeField]Image cardImage;

    EffectCardsPanel effectCardsPanel;
    public void Init(Card card,EffectCardsPanel effectCardsPanel)
    {
        this.card = card;

        this.effectCardsPanel = effectCardsPanel;

        cardName.text = card.name;

        cardAtk.text = card.atk.ToString();
    }

}
