using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class sample : MonoBehaviour
{
    public Button confirmButton;
    public TMP_InputField inputField;

    void Start()
    {
        StartCoroutine(GameFlow());
    }

    private IEnumerator GameFlow()
    {
        string result = null;

        // 注册回调：当按钮点击时，把输入传回 result
        confirmButton.onClick.AddListener(() =>
        {
            result = inputField.text;
        });

        // 等待直到 result 不为空
        yield return new WaitUntil(() => result != null);

        Debug.Log("玩家输入: " + result);
    }
}