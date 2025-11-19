using UnityEngine;
using TMPro;          // 使用 TextMeshPro 需要引用
using UnityEngine.UI; // 按钮

public class CardCreator : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField; // 拖拽你的 TMP_InputField 到这里
    public Button submitButton;       // 拖拽按钮

    private void Start()
    {
        // 注册按钮点击事件
        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        // 读取输入框文本
        string inputText = inputField.text;

        // 输出到控制台
         LogCenter.Log("输入内容: " + inputText);

        // 如果想清空输入框：
        // inputField.text = "";
    }
}