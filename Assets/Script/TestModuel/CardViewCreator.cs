using DG.Tweening;
using UnityEngine;

public class CardViewCreator : QuickInstance<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public CardView CreateCardView(Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);

        Vector3 originalScale = cardView.transform.localScale;

        cardView.transform.localScale = Vector3.zero;

        cardView.transform.DOScale(originalScale, 0.15f);

        return cardView;
    }
}