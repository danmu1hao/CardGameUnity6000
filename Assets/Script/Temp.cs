using DG.Tweening;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public RectTransform rectTransform;

    void Start()
    {
        // 移动 UI 元素到新的位置 (基于锚点的局部坐标)
        rectTransform.DOAnchorPos(new Vector2(100, -50), 1f)
            .SetEase(Ease.OutBack);
            
        // 缩放 UI 元素
        rectTransform.DOScale(Vector3.one * 1.5f, 0.5f)
            .SetLoops(2, LoopType.Yoyo); // 放大再缩小
            
        // 改变宽度和高度 (例如：扩大到 300x150)
        rectTransform.DOSizeDelta(new Vector2(300, 150), 0.8f);
    }
}