using UnityEngine;

public class LogCenter 
{
    public static void Log(string content)
    {
        Debug.Log(content);
    }
    public static void Info(string msg)
    {
        Debug.Log(msg);
    }
    public static void Warning(string msg)
    {
        Debug.LogWarning(msg);
    }

    public static void Error(string msg)
    {
        Debug.LogError(msg);
    }
}
