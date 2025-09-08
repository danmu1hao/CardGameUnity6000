using UnityEngine;

public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 已存在 → 销毁重复的
            return;
        }
        Instance = this as T;
        DontDestroyOnLoad(gameObject); // 如果不需要跨场景，可以删掉这行
    }
}
