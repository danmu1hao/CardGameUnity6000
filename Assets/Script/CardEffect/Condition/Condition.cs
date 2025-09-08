using System.Collections.Generic;

public class Condition
{
    public string[] conditionList;
    public CardEffect cardEffect;
    public Condition(string[] condition,CardEffect cardEffect)
    {
        conditionList = condition;
        this.cardEffect = cardEffect;
    }
}