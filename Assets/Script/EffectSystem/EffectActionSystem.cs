/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionTiming
{
    PRE,   // 在动作执行前触发
    POST   // 在动作执行后触发
}

public class EffectActionSystem : SimpleSingleton<EffectActionSystem>
{
    /// <summary>
    /// performers preSubs postSubs eaction
    /// </summary>
    
    private List<CardEffect> reactions = null;
    public bool IsPerforming { get; private set; } = false;
    // 不是自己去找，而是让可触发的对象自己订阅？真妙
    private static Dictionary<Type, List<Action<CardEffect>>> preSubs = new();
    private static Dictionary<Type, Func<CardEffect, IEnumerator>> performers = new();

    // 执行一个游戏动作（GameAction），并在完成后调用回调
    public void Perform(CardEffect action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming) return; // 若当前已有动作在执行，则直接返回

        IsPerforming = true; // 标记为正在执行

        // 启动协程执行流程
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false; // 执行完毕后重置状态
            OnPerformFinished?.Invoke(); // 调用完成回调
        }));
    }
    // 主流程：顺序执行 前置、执行中、后置 的三个阶段
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // 前置反应阶段
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs); // 执行前置订阅者
        yield return PerformReactions(); // 执行前置反应

        // 执行阶段
        reactions = action.PerformReactions;
        yield return PerformPerformer(action); // 调用执行器逻辑
        yield return PerformReactions(); // 执行执行阶段的附加反应

        // 后置反应阶段
        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs); // 执行后置订阅者
        yield return PerformReactions(); // 执行后置反应

        OnFlowFinished?.Invoke(); // 通知流程完成
    }
    // 添加一个反应动作到当前动作列表
    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }



    // 执行当前阶段的所有反应（递归调用 Flow）
    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }

    // 执行当前 GameAction 类型对应的执行器逻辑
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    // 执行所有订阅了该类型动作的回调函数
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }
        }
    }

    #region subscribe
    // 注册一个动作执行器（将某个 GameAction 类型绑定到执行逻辑）
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);

        if (performers.ContainsKey(type))
            performers[type] = wrappedPerformer;
        else
            performers.Add(type, wrappedPerformer);
    }

    // 移除指定类型的执行器
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
            performers.Remove(type);
    }

    // 注册一个反应函数，根据 ReactionTiming（前置/后置）分类存储
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        void wrappedReaction(GameAction action) => reaction((T)action);

        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new List<Action<GameAction>> { wrappedReaction });
        }
    }

    // 取消注册的反应函数
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

        if (subs.ContainsKey(typeof(T)))
        {
            void wrappedReaction(GameAction action) => reaction((T)action);
            subs[typeof(T)].Remove(wrappedReaction);
        }
    }


    #endregion
  



}
*/
