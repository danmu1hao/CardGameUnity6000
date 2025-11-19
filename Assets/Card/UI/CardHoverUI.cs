using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image target;          // 想要变色的 Image（可为空）
    public Color hover = Color.yellow;
    private Color _origin;

    void Awake()
    {
        if (target == null) target = GetComponent<Image>();
        if (target != null) _origin = target.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
         LogCenter.Log("Pointer Enter: " + gameObject.name);
        if (target != null) target.color = hover;
        // TODO: 显示 Tooltip、播放动画、改变光标等
    }

    public void OnPointerExit(PointerEventData eventData)
    {
         LogCenter.Log("Pointer Exit: " + gameObject.name);
        if (target != null) target.color = _origin;
        // TODO: 隐藏 Tooltip、恢复状态
    }
}