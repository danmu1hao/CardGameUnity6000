using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Processor : MonoBehaviour
{
    public static Processor instance { get; private set; }
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Test() {
        TimingEnum timingEnum= TimingEnum.Test;
        process process = new process(timingEnum);
        CheckEffect(process);

    }

    public Stack<Effect> effectStack = new Stack<Effect>();
    public void CheckEffect(process process)
    {
        foreach (var target in GameManager.instance.players)
        {
            foreach (var card in target.Cards)
            {
                foreach (var effect in card.Effects)
                {
                    if (CheckSingleEffect(process))
                    {
                        effectStack.Push(effect);
                    }
                }
            }
        }

    }
    #region  Check
    public bool CheckSingleEffect(process process)
    {
        if (!CheckTiming())
        {
            return false;
        }
        if (!CheckCondition())
        {
            return false;
        }
        if (!CheckCost())
        {
            return false;
        }
        return true;
    }
    public bool CheckTiming()
    {
        return true;
    }
    public bool CheckCondition()
    {
        return true;
    }
    public bool CheckCost()
    {
        return true;
    }
    #endregion
    
    public void ShowCanEffectCards()
    {
        foreach (var effect in effectStack)
        {
            Debug.Log(effect);
        }
    }

}
