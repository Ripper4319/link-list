using System;
using UnityEngine;

public class EnemyState
{
    public string Name;
    public Action OnEnter;
    public Action OnUpdate;
    public Action OnExit;
    public EnemyState NextState;

    public EnemyState(string name, Action onEnter, Action onUpdate, Action onExit)
    {
        Name = name;
        OnEnter = onEnter;
        OnUpdate = onUpdate;
        OnExit = onExit;
        NextState = null;
    }
}

public class EnemyFSM : MonoBehaviour
{
    private EnemyState currentState;

    public void SetInitialState(EnemyState state)
    {
        currentState = state;
        currentState?.OnEnter?.Invoke();
    }

    public void UpdateFSM()
    {
        if (currentState == null) return;

        currentState.OnUpdate?.Invoke();

        if (currentState.NextState != null)
        {
            TransitionTo(currentState.NextState);
        }
    }

    public void TransitionTo(EnemyState newState)
    {
        currentState?.OnExit?.Invoke();
        currentState = newState;
        currentState?.OnEnter?.Invoke();
    }
}
