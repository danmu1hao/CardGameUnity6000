using UnityEngine;

public class UIMethod
{
    private static UIMethod instance;
    public static UIMethod GetInstance()
    {
        if (instance == null)
        {
            instance = new UIMethod();
        }
        return instance; 
    }

    public GameObject FindCanvas()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
             LogCenter.LogError("Canvas not found");
        }
        return canvas;
    }
}
