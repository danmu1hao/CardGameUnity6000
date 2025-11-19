using DG.Tweening;
using UnityEngine;

public class Temp : MonoBehaviour
{

    void Start()
    {
        Debug.Log(CardEnums.TryGetEnum<CardEnums.ObjectEnum>("asda"));
    }
}