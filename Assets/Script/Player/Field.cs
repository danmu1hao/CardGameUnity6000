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
    
    public int fieldIndex;
    public Field(Player player,int index)
    {
        this.player = player;
        soulCards = new List<Card>();
        this.fieldIndex = index;
    }

}
