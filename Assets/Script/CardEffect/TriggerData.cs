using System.Collections.Generic;

public class TriggerData
{
    public Card Source;
    public List<Card> MutiTarget;

    public TriggerData(Card source, List<Card> mutiTarget)
    {
        Source = source;
        MutiTarget = mutiTarget;
    }

}