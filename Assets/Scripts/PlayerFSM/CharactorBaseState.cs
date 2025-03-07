using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharactorBaseState : EntityBaseState
{
    protected Animator animator;
    public CharactorBaseState(FSM fsm) : base(fsm)
    {
        animator = fsm.GetComponent<Animator>();
    }
}
