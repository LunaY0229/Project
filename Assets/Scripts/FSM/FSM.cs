using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T> : FSM where T : Enum
{
    /// <summary>
    /// ״̬
    /// </summary>
    protected Dictionary<T, CharactorBaseState> states = new Dictionary<T, CharactorBaseState>();
    protected T currentState;
    protected Animator animator;
    public CharacterController characterController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public void ChangState(T newState, string child = "")
    {
        if (states.ContainsKey(currentState))
            states[currentState].OnExit();

        currentState = newState;

        if (!string.IsNullOrEmpty(child))
        {
            if (states[currentState] is BaseSubState)
            {
                (states[currentState] as BaseSubState).SetEnterChild(child);
            }
        }

        states[currentState].OnEnter();
    }
}

public abstract class FSM: MonoBehaviour
{
    public virtual void Demage(Transform attacker, float hit, float force)
    {

    }
}
