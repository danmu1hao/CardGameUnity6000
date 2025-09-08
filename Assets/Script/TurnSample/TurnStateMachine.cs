// using System.Collections.Generic;

// public enum TurnStateEnum
// {
//     TurnStart, Battle, TurnEnd
// }
// public class TurnStateMachine
// {
//     private Dictionary<TurnStateEnum, TurnStateBase> states = new Dictionary<TurnStateEnum, TurnStateBase>();
    
//     // 这里初始化Dict
//     public void Init()
//     {
//         states[TurnStateEnum.TurnStart] = new TurnStartState(this, states);
//         states[TurnStateEnum.Battle] = new BattleState(this, states);
//         states[TurnStateEnum.TurnEnd] = new TurnEndState(this, states);
//     }

//     private TurnStateBase currentState;

//     public void SetState(TurnStateBase newState)
//     {
//         currentState?.Exit();
//         currentState = newState;
//         currentState?.Enter();
//     }

//     public void Update()
//     {
//         currentState?.Update();
//     }
// }