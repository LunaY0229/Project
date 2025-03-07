using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseSubState<T> : BaseSubState where T : Enum
{
    protected Dictionary<T, CharactorBaseState> allChildState = new Dictionary<T, CharactorBaseState>();

    protected T currentState;
    protected BaseSubState(FSM fsm) : base(fsm)
    {

    }

    public override void SetEnterChild(string childName) 
    {
        if (string.IsNullOrEmpty(childName)) return;

        foreach (var key in allChildState.Keys)
        {
            if (Enum.GetName(typeof(T), key).Equals(childName))
            {
                ChangeState(key);
                break;
            }
        }
    }

    public void ChangeState(T newState)
    {
        if (allChildState.ContainsKey(currentState))
            allChildState[currentState].OnExit();

        currentState = newState;

        if (allChildState.ContainsKey(currentState))
            allChildState[currentState].OnEnter();
    }

    public override void OnFixedUpdate()
    {
        if (allChildState.ContainsKey(currentState))
            allChildState[currentState].OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        if(allChildState.ContainsKey(currentState))
            allChildState[currentState].OnUpdate();
    }
}

public abstract class BaseSubState : CharactorBaseState
{
    protected BaseSubState(FSM fsm) : base(fsm) { }
    public virtual void SetEnterChild(string childName) { }
}
