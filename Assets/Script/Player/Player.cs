using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player:IClassResolver
{
    public int playerId;

    public int Turn=0;

    public List<Field> fields = new List<Field>(3);
    public List<Card> inFieldCards => fields.Select(f => f.card).Where(c => c != null).ToList();
    public List<Card> inSoulCards => 
        fields.SelectMany(f => f.soulCards).Where(c => c != null).ToList();



    #region �����б�

    // inDeckCards: ���ƿ��еĿ���
    // inHandCards: �������еĿ���
    // inBattleFieldCards : ��ս���ϵĿ��ƣ����ڳ��ϵĿ��ƣ�
    // inDamageCards: ���˺����Ŀ���
    // inDiscardCards: ���������еĿ���
    // inSoulCards: ��������Ŀ���


    public List<Card> inDeckCards = new List<Card>(),
        inHandCards = new List<Card>(),
        inDamageCards = new List<Card>(),
        inBattleFieldCards = new List<Card>();


    public List<Card> inDiscardCards = new List<Card>();
    #endregion





    public List<Card> AllCards
    {
        get
        {
            return inDeckCards.Concat(inHandCards).Concat(inFieldCards).Concat(inSoulCards).Concat(inDamageCards).Concat(inDiscardCards).ToList();
        }
    }

    public Player(int playerId)
    {
        this.playerId = playerId;

    }

    public string TryResolveCard(string resolveContent)
    {
        Debug.Log(resolveContent);
        return null;
    }
}
