using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

/// <summary>
/// 用于 Canvas UI 卡牌的排布（基于 Spline 弧形）
/// 使用方式：StartCoroutine(handView.AddCard(cardView));
/// cardView 必须是 Canvas 下的 UI（RectTransform）
/// </summary>
public class HandCardLayoutUI : MonoBehaviour
{
    
    //TODO UI没有重排啊
    // [SerializeField] private SplineContainer splineContainer; 暂时放弃曲线
    [FormerlySerializedAs("cards")] [SerializeField]private List<HandCardDisplay> handCarss = new();

    public IEnumerator AddCard(HandCardDisplay cardView)
    {
        handCarss.Add(cardView);
        Debug.Log(handCarss.Count);
        yield return UpdateCardPositions(0.15f);
    }

    public IEnumerator DeleteCard(HandCardDisplay cardView)
    {
        handCarss.Remove(cardView);
        Debug.Log(handCarss.Count);
        yield return UpdateCardPositions(0.15f);
    }

    [SerializeField] private float cardSpacing; 
    private IEnumerator UpdateCardPositions(float duration)
    {
        if (handCarss.Count == 0) yield break;
        float firstCardPosition= - cardSpacing*((handCarss.Count-1f)/2f);
        for (int i = 0; i < handCarss.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;

            // UI 卡牌位置：取 x,y，作为 anchoredPosition
            Vector2 targetPos = new Vector2(p,0);
            
            handCarss[i].gameObject.GetComponent<RectTransform>().DOAnchorPos(targetPos, duration);

        }

        yield return new WaitForSeconds(duration);
    }

    #region OldDesign

        // 我们暂时放弃曲线的思路
    // private IEnumerator UpdateCardPositions(float duration)
    // {
    //     if (cards.Count == 0) yield break;
    //
    //     float cardSpacing = 1f / 10f;
    //     float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
    //     Spline spline = splineContainer.Spline;
    //
    //     for (int i = 0; i < cards.Count; i++)
    //     {
    //         float p = firstCardPosition + i * cardSpacing;
    //         Vector3 splinePosition = spline.EvaluatePosition(p);
    //         Vector3 forward = spline.EvaluateTangent(p);
    //         Vector3 up = spline.EvaluateUpVector(p);
    //
    //         Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
    //
    //         // UI 卡牌位置：取 x,y，作为 anchoredPosition
    //         Vector2 targetPos = new Vector2(splinePosition.x, splinePosition.y);
    //         
    //         cards[i].DOAnchorPos(targetPos, duration);
    //         cards[i].DORotate(rotation.eulerAngles, duration);
    //     }
    //
    //     yield return new WaitForSeconds(duration);
    // }

    #endregion
}