// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// 这里不对，应该是一个currentstate一个state的类字典
// public abstract class TurnStateBase
// {
//     private TurnStateMachine machine;
//     private Dictionary<TurnStateEnum, TurnStateBase> states = new Dictionary<TurnStateEnum, TurnStateBase>();
//     public TurnStateBase(TurnStateMachine machine, Dictionary<TurnStateEnum, TurnStateBase> states)
//     {
//         this.machine = machine;
//         this.states = states;
//     }

//     public abstract void Enter();
//     public abstract void Update();
//     public abstract void Exit();
// }

// public class TurnStartState : TurnStateBase
// {
//     public TurnStartState(TurnStateMachine machine, Dictionary<TurnStateEnum, TurnStateBase> states) : base(machine, states)
//     {
//     }

//     public override void Enter()
//     {
//          LogCenter.Log("进入回合开始阶段：准备出牌");

//         BattleSystem.Instance.DrawCard();
        
//     }

//     public override void Update()
//     {
//     }

//     public override void Exit()
//     {
//         machine.SetState(new BattleState(machine));
//     }
// }

// public class BattleState : TurnStateBase
// {
//     public BattleState(TurnStateMachine machine, Dictionary<TurnStateEnum, TurnStateBase> states) : base(machine, states)
//     {
//     }


//     public override void Enter()
//     {
//         throw new NotImplementedException();
//     }

//     public override void Update()
//     {
//         throw new NotImplementedException();
//     }

//     public override void Exit()
//     {
//         throw new NotImplementedException();
//     }
// }

// public class TurnEndState : TurnStateBase
// {
//     public TurnEndState(TurnStateMachine machine, Dictionary<TurnStateEnum, TurnStateBase> states) : base(machine, states)
//     {
//     }

//     public override void Enter()
//     {
        
//         throw new NotImplementedException();
//     }

//     public override void Exit()
//     {
        
//         if (BattleSystem.Instance.currentPlayer==BattleSystem.Instance.Player1)
//         {
//              LogCenter.Log("玩家1回合结束：切换玩家");
//         }
//         else
//         {
//              LogCenter.Log("玩家2回合结束：切换玩家");
//         }
        
        
//         BattleSystem.Instance.ChangerPlayer();
//         machine.SetState(states[TurnStateEnum.TurnStart]);
//         throw new NotImplementedException();
//     }

//     public override void Update()
//     {
//         throw new NotImplementedException();
//     }
// }
