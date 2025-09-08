using UnityEngine;
using UnityEngine.UI;

public class NewArrow : MonoBehaviour
{
    public RectTransform arrow; // 手动拖进 Image 对象（箭头图）
    private Vector2 startPointScreenPos;
    private Vector2 endPointScreenPos;
    private Vector2 arrowDirection;
    private float arrowLength;
    private float angle;

    public Canvas canvas; // 当前 UI 所属的 Canvas（必须是 Render Mode: Screen Space - Overlay）

    void Start()
    {
        if (arrow == null)
            arrow = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 1️⃣ 获取当前 UI 物体在屏幕上的坐标作为起点
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Camera.main.WorldToScreenPoint(transform.position),
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out startPointScreenPos
        );

        // 2️⃣ 鼠标坐标转为本地坐标（同一个坐标空间）
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out endPointScreenPos
        );

        // 3️⃣ 计算方向、长度、角度
        arrowDirection = endPointScreenPos - startPointScreenPos;
        arrowLength = arrowDirection.magnitude;
        angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;

        // 4️⃣ 更新箭头
        arrow.sizeDelta = new Vector2(arrowLength, arrow.sizeDelta.y);
        arrow.localEulerAngles = new Vector3(0, 0, angle);
        arrow.localPosition = startPointScreenPos + arrowDirection / 2; // 箭头居中显示
    }
}