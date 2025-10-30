using System.Collections.Generic;

public class TriggerData
{
    public Card Source;
    public List<Card> MutiTarget;
    public Player TargetPlayer;
    //对象是复数卡牌
    public TriggerData(Card source, List<Card> mutiTarget)
    {
        Source = source;
        MutiTarget = mutiTarget;
    }
    //对象是玩家
    public TriggerData(Card source, Player targetPlayer)
    {
        Source = source;
        this.TargetPlayer = targetPlayer;
    }
    //没对象
    public TriggerData(Card source)
    {
        Source = source;

    }
    public TriggerData(Player player)
    {
        this.TargetPlayer = player; 
    }
}