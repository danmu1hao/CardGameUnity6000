using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class BattleSystem : QuickInstance<BattleSystem>
{
    
    public void BattleStart()
    {
        Init();
        // TurnState = new TurnStartState(new TurnStateMachine());
        GameManager.instance.PrepareDeck();
        StartTurn();
        BattleSystemUI.instance.AllotField();
    }
    public List<Player> players = new List<Player>();
    /// <summary>
    /// 目前默认玩家1是主角
    /// </summary>
    public Player Player1 = new Player(0);
    public  Player Player2 = new Player(1);

    public List<Card> allCardsInBattle => players.SelectMany(p => p.AllCards).ToList();

    public Player GetPlayer(int playerID,bool isPlayer)
    {
        if (Player1.playerId == playerID)
        {
            return Player1;
        }
        else
        {
            return Player2;
        }
    }
    public  Player currentPlayer;

    public void Init()
    {
        Player1 = new Player(0); // ✅ 你应该在这里初始化
        Player2 = new Player(1);
        currentPlayer = Player1;
        players.Add(Player1);
        players.Add(Player2);
        for (int i = 0; i < 3; i++)
        {
            Field field = new Field(Player1,i);
            Player1.fields.Add(field); 
            Field field2 = new Field(Player2,i);
            Player2.fields.Add(field2);
        }
    }

    public  void CreateDeck()
    {
        
    }
    void Update()
    {
        if (chooseSlot && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Input.mousePosition, Vector2.zero);

            foreach (var h in hits)
            {
                if (h.collider != null)
                {
                    FieldUI field = h.collider.GetComponent<FieldUI>();
                    if (field != null)
                    {
                        CheckSummon(h);
                        break; // 如果只要第一个找到的 FieldUI
                    }
                }
            }
        }
    }



    public void ResetUseCard()
    {
        chooseSlot = false;
        currentCard.cardModel.GetComponent<CardMouseInHand>().MoveBack();
        currentCard = null;
        
    }

    #region 战斗系统

    public async Task BattleConfirm(Card attker, GameObject hitObj)
    {
        //玩家还是空slot
        if ( hitObj.GetComponent<FieldUI>()!=null)
        {
            Player enemy;
            if (hitObj.GetComponent<FieldUI>().field.player == currentPlayer)
            {
                return; 
            }
            enemy = hitObj.GetComponent<FieldUI>().field.player;
            // 确认对面有没有怪
            if (enemy.inFieldCards.Count != 0)
            {
                return;
            }
            TriggerData triggerData = new TriggerData(attker, enemy);

            // 等待效果执行完成
            await EffectSystem.instance.TimingTrigger(CardEnums.TimingEnum.when_attack, triggerData);

            // 效果全部处理完之后再继续战斗
            BattleConfirmTwo(attker, enemy);
        }

        if (hitObj.GetComponent<UnitCardDisplay>()!=null)
        {
            Card defender = hitObj.GetComponent<UnitCardDisplay>().card;
            if (attker.player == currentPlayer &&
                attker.player != defender.player &&
                attker.hasAttacked == false)
            {
                TriggerData triggerData = new TriggerData(attker, new List<Card> { defender });

                // 等待效果执行完成
                await EffectSystem.instance.TimingTrigger(CardEnums.TimingEnum.when_attack, triggerData);

                // 效果全部处理完之后再继续战斗
                BattleConfirmTwo(attker, defender);
            }
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
    public  void BattleConfirmTwo(Card attker, Player player)
    {
        if (attker.cancelFight)
        {
            attker.cancelFight = false;
            attker.hasAttacked = true;
            return;
        }
        Battle(attker, player);
    }
    public  void Battle(Card attker, Card defender)
    {
        if (attker.atk > defender.atk)
        {
            DestroyFieldCard(defender);
            DamagePlayer(defender.player, attker.atk - defender.atk);
        }
        else if (attker.atk < defender.atk)
        {
            DestroyFieldCard(attker);
            DamagePlayer(attker.player, defender.atk - attker.atk);
        }
        else if (attker.atk == defender.atk)
        {
            DestroyFieldCard(attker);
            DestroyFieldCard(defender);
        }

        attker.hasAttacked = true;
    }
    public  void Battle(Card attker, Player player)
    {
        DamagePlayer(player, attker.atk );
        attker.hasAttacked = true;
    }



    #endregion

    #region 使用卡牌
    private Card currentCard;
    public bool chooseSlot;
    public async Task<bool> UseCardCheck(Card card)
    {
        // 用卡是一个流程
        if (card.player != currentPlayer || card.player==null)
        {
            return false;
        }

        if (card.sacrificeCost > 0)
        {
            Debug.Log("开始血祭");
            if (SacrificeCheck(card))
            {
                card.sacrificeTargets=await SacrificeCosts(card);
                if (card.sacrificeTargets.Count==0)
                {
                    print("血祭失败");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else if (card.soulCost > 0)
        {
            Debug.Log("开始灵魂召唤"+card.soulCost+"灵魂消耗"+card.player.inSoulCards.Count+"灵魂");
            if (SoulSummonCheck(card))
            {
                card.soulTargets=await SoulCosts(card);
                if (card.soulTargets.Count==0)
                {
                    Debug.Log("灵魂失败");
                    return false;
                }
                else
                {
                    Debug.Log("灵魂选择成功");
                }
            }
            else
            {
                return false;
            }
            
        }
        // BattleSystemUI.instance.UseCardCheck(player, card);
        // card.cardModel.GetComponent<CardMouseInHand>().WaitingForClickUI();
        currentCard = card;
        chooseSlot = true;
        return true;
    }
    public  void UseCard(Player player, Card card,GameObject field=null)
    {
        ExecuteSummonCost(card);
        if (card.CardTypeEnum==CardEnums.CardTypeEnum.Monster)
        {
            SummonCard(card,field);
        }
        BattleSystemUI.instance.UseCard(card,field);
    }

    void ExecuteSummonCost(Card card)
    {
        if (card.soulTargets.Count > 0)
        {
            foreach (var soulCard in card.soulTargets)
            {
                Debug.Log(soulCard.name+"进入墓地");
                soulCard.Move(CardEnums.CardStateEnum.InCemetery);
            }
        }

        if (card.sacrificeTargets.Count > 0)
        {
            foreach (var sacrificeCard in card.sacrificeTargets)
            {
                Debug.Log(sacrificeCard.name+"进入墓地");
                DestroyFieldCard(sacrificeCard);
            }

        }
    }

    public bool SoulSummonCheck(Card card)
    {
        // 确认灵魂是否足够

        List<Card> soulCards = card.player.inSoulCards;
        if (soulCards.Count>= card.soulCost)
        {
            return true;
        }

        return false;
    }


    public  bool SacrificeCheck(Card card)
    {
        // 确认祭品是否足够
        Debug.Log("祭品需求"+card.sacrificeCost+ "场上卡牌数量"+card.player.inFieldCards.Count);
        if (card.player.inFieldCards.Count >= card.sacrificeCost)
        {
            return true;
        }

        return false;
    }

    async Task<List<Card>> SacrificeCosts( Card card)
    {
        //进入一个选择界面
        List<Card> sacrificeCards = FindTarget(c => c.player == currentPlayer && c.state == CardEnums.CardStateEnum.InField);
        Debug.Log("找到祭品"+sacrificeCards.Count);
        List<Card> chosen = await UIManager.instance.OpenSelectPanel(sacrificeCards);

        /*foreach (var chosCard in chosen)
        {
            DestroyFieldCard(chosCard);
        }*/

        return chosen;
    }
    async Task<List<Card>> SoulCosts( Card card)
    {
        //进入一个选择界面
        List<Card> soulCards = FindTarget(c => c.player == currentPlayer && c.state == CardEnums.CardStateEnum.InSoul);
        Debug.Log("找到灵魂"+soulCards.Count);
        List<Card> chosen = await UIManager.instance.OpenSelectPanel(soulCards);

        return chosen;
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


    #endregion
    #region 卡牌系统

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
        Debug.Log("执行抽卡");
        if (player.inDeckCards.Count<=0)
        {
            return;
        }
       Card card = player.inDeckCards[0];
       card.Move(CardEnums.CardStateEnum.InHand);
        if (player==Player1)
        {
            BattleSystemUI.instance.DrawCard(true,player,card);
        }
        else
        {
            BattleSystemUI.instance.DrawCard(false,player,card);
        }
        
    }

    public void DisCard(Player player,Card card)
    {
        Debug.Log("执行丢卡");
        if (!player.inHandCards.Contains(card))
        {
            Debug.Log("玩家手里并不存在这张卡");
            return;
        }

        card.Move(CardEnums.CardStateEnum.InCemetery);
        if (player==Player1)
        {
            BattleSystemUI.instance.DeleteCard(card);
        }
        else
        {
            BattleSystemUI.instance.DeleteCard(card);
        }
    }
    #endregion

    #region 回合系统

    public void StartTurn()
    {
        DrawCard(currentPlayer);
        // 重置攻击
        foreach (var card in currentPlayer.inFieldCards)
        {
            card.hasAttacked=false;
        }

        TriggerData triggerData = new TriggerData(currentPlayer);
        EffectSystem.instance.TimingTrigger(CardEnums.TimingEnum.when_turn_start,triggerData);
    }
    public void EndTurn()
    {
        ChangerPlayer();
        StartTurn();
    }
        

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


    #region 破坏与召唤

    /// <summary>
    /// 这个方法是用来确认对象格子是否可以用于召唤的
    /// </summary>
    /// <param name="hit"></param>
    private void CheckSummon(RaycastHit2D hit)
    {
        Field field = hit.collider.gameObject.GetComponent<FieldUI>().field;
        Debug.Log($"Hit UI element: {hit.collider.gameObject.name}");
        // 是当前玩家 对象没卡 || 对象有卡 但是，那个卡将会被牺牲


        if (field.player == currentPlayer && field.card == null
            || (field.card!=null && currentCard.sacrificeTargets.Contains(field.card)))
        {
            chooseSlot = false;
            GameObject selectSlot = hit.collider.gameObject;

            UseCard(currentCard.player, currentCard,selectSlot);
            currentCard = null;
        }

    }
    
    public  void DestroyFieldCard(Card card)
    {
        //状态处理
        card.Move(CardEnums.CardStateEnum.InSoul);
        Destroy(card.cardModel);
        Debug.Log(card.player.inSoulCards.Count);
    }

    public void SummonCard(Card card,GameObject field)
    {
        // //状态处理
        // if (field!=null)
        // {
        //     field.GetComponent<FieldUI>().Regist(card);
        // }
        
        card.Move(CardEnums.CardStateEnum.InField,field.GetComponent<FieldUI>().field);
        card.hasAttacked = true;
    }

    #endregion
}
