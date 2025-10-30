using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field
{
    public Player player;
    //暂时用不到？
    public GameObject fieldModel;
    
    public Card card;
    public List<Card> soulCards=new List<Card>();
    public Field(Player player)
    {
        this.player = player;
        soulCards = new List<Card>();
    }

}
