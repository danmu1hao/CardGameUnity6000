  using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoPanel : MonoBehaviour
{
    public static CardInfoPanel Instance;
    void Awake()
    {
        Instance = this;
    }
    [SerializeField] GameObject panel;
    
    private Vector3 mouseDownPosition;

    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI attack;
    [SerializeField] Image image;

    public void UpdatePanelUI(Card card)
    {
        // Update the text fields with CardConfig data
        name.text = card.CardConfig.name;
        // TODO 多效果显示
        description.text = card.CardConfig.effectDescription[0];

        // 两种UI显示
        //cost.text = card.cardConfig.cost.ToString();
        
        attack.text = card.CardConfig.atk.ToString();
        if (card.cardImage != null)
        {
            // 找到了图片，使用它
            image.sprite = card.cardImage;
        }


    }
    [SerializeField] private float longPressTime = 0.5f; // 长按阈值（秒）
    [SerializeField]  float mouseDownTimer = 0f;
    [SerializeField]  bool isHolding = false;
    public  float DragThreshold = 0.1f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition = Input.mousePosition;
            isHolding = true;
            mouseDownTimer = 0f;
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            mouseDownTimer += Time.deltaTime;
            float moveDistance = Vector3.Distance(mouseDownPosition, Input.mousePosition);

            if (mouseDownTimer >= longPressTime && moveDistance < DragThreshold)
            {
                isHolding = false; // 只触发一次

                RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                     LogCenter.Log("碰撞到物体");
                    Card card = ReadCardData.ReadCard(hit.collider.gameObject);
                    if (card != null)
                    {
                        panel.SetActive(true);
                        UpdatePanelUI(card);
                         LogCenter.Log("长按显示面板");
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // 如果是短按，并且没有触发长按 → 关闭面板
            if (mouseDownTimer < longPressTime)
            {
                panel.SetActive(false);

            }

            isHolding = false;
            mouseDownTimer = 0f;
        }
    }


}
