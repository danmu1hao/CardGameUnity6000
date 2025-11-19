using UnityEngine;

public class LogCenter 
{
    public static void Log(string content)
    {
         Debug.Log(content);
    }
    public static void Log(int content)
    {
         Debug.Log(content.ToString());
    }
    
    public static void Info(string msg)
    {
         Debug.Log(msg);
    }
    public static void LogWarning(string msg)
    {
         Debug.LogWarning(msg);
    }

    public static void LogError(string msg)
    {
         Debug.LogError(msg);
    }
}
