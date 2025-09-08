using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class BattleSystem : QuickInstance<BattleSystem>
{

    public void Start()
    {
        Player1 = new Player(0); // ✅ 你应该在这里初始化
        Player2 = new Player(1);
        currentPlayer = Player1;
        // TurnState = new TurnStartState(new TurnStateMachine());
    }
    
    public Player Player1 = new Player(0);
    public  Player Player2 = new Player(1);
    public  Player currentPlayer;


    public  void CreateDeck()
    {

    }
    
    #region 战斗

    public async Task BattleConfirm(Card attker, Card defender)
    {
        if (attker.player == currentPlayer &&
            attker.player != defender.player &&
            attker.hasAttacked == false)
        {
            TriggerData triggerData = new TriggerData(attker, new List<Card> { defender });

            // 等待效果执行完成
            await EffectSystem.Instance.CheckEffectWithTiming(Timing.TimingList.when_attack, triggerData);

            // 效果全部处理完之后再继续战斗
            BattleConfirmTwo(attker, defender);
        }
    }


    public  void BattleConfirmTwo(Card attker, Card defender)
    {
        if (attker.cancelFight)
        {
            attker.cancelFight = false;
            attker.hasAttacked = true;
            return;
        }

        Battle(attker, defender);
    }

    public  void Battle(Card attker, Card defender)
    {
        if (attker.atk > defender.atk)
        {
            DestroyCard(defender);
            DamagePlayer(defender.player, attker.atk - defender.atk);
        }
        else if (attker.atk < defender.atk)
        {
            DestroyCard(attker);
            DamagePlayer(attker.player, defender.atk - attker.atk);
        }
        else if (attker.atk == defender.atk)
        {
            DestroyCard(attker);
            DestroyCard(defender);
        }

        attker.hasAttacked = true;
    }

    public  void DestroyCard(Card card)
    {
        BattleSystemUI.instance.DestroyCard(card);
    }


    #endregion

    #region 使用卡牌

    public  void UseCardCheck(Player player, Card card)
    {
        if (card.sacrificeCost > 0)
        {
            SacrificeCheck(card);
        }
        else if (card.soulCost > 0)
        {
            SoulSummonCheck(card);
        }

    }

    public  void SoulSummonCheck(Card card)
    {
        // 确认灵魂是否足够
        List<Card> soulCards = FindTarget(CardInSoulField);
        if (soulCards.Count> card.soulCost)
        {

        }
    }
    public  bool CardInBattleField(Card card)
    {
        if(card.CardState==Card.CardStateEnum.InBattle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public  bool CardInSoulField( Card card)
    {
        if (card.CardState == Card.CardStateEnum.InSoul)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public  void SacrificeCheck(Card card)
    {
        // 确认祭品是否足够

    }
    public  List<Card> FindTarget(Func<Card, bool> targetCondition)
    {
        List<Card> targetCards = new List<Card>();
        foreach (var card in Player1.AllCards.Concat(Player2.AllCards))
        {
            if (targetCondition(card))
            {
                targetCards.Add(card);
            }
        }
        return targetCards; 
    }
    public  void UseCard(Player player, Card card,GameObject field=null)
    {

        if (field!=null)
        {
            field.GetComponent<FieldUI>().Regist(card);
        }
        
        card.Move(Card.CardStateEnum.InBattle);
        
        BattleSystemUI.instance.UseCard(player, card);


    }

    #endregion
    #region 抽牌

    public  void DrawCard(Card card)
    {
        if (currentPlayer==Player1)
        {
            BattleSystemUI.instance.DrawCard(true,currentPlayer,card);
        }
        else
        {
            BattleSystemUI.instance.DrawCard(false,currentPlayer,card);
        }
    }
    
    public  void DrawCard(Player player)
    {
        if (player.inDeckCards.Count<=0)
        {
            return;
        }
       Card card = player.inDeckCards[0];
       card.Move(Card.CardStateEnum.InHand);
        if (player==Player1)
        {
            BattleSystemUI.instance.DrawCard(true,player,card);
        }
        else
        {
            BattleSystemUI.instance.DrawCard(false,player,card);
        }
        
    }

    #endregion

    #region Turn


    public  void ChangerPlayer()
    {
        if (currentPlayer == Player1)
        {
            currentPlayer = Player2;
        }else
        {
            currentPlayer = Player1;
        }

        foreach (var card in currentPlayer.inFieldCards)
        {
            card.hasAttacked = false;
        }
    }
    #endregion

    #region 伤害区

    public  bool gameOver;
    //把n张卡放到伤害区
    
    public  void DamagePlayer(Player player,int damage)
    {
        if(player.inDeckCards.Count<damage)
        {
            return;
        }
        List<Card> cards = new List<Card>();    
        for (int i = 0; i < damage; i++)
        {
            cards.Add(player.inDeckCards[0]);
            player.inDeckCards.RemoveAt(0);
        }
        BattleSystemUI.instance.AddCardToDamageArea(cards);
        
        CheckGameOver();

    }

    //没法放牌也是输
    public  void CheckGameOver()
    {
        if (Player1.inDamageCards.Count >= 10)
        {
            gameOver=true;
            
            
        }else if (Player2.inDamageCards.Count >= 10)
        {
            gameOver=true;
        }
    }
    #endregion


}
