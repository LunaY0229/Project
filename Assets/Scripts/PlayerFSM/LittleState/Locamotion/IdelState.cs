using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : CharactorBaseState
{
    protected new PlayerFSM fsm;
    private float translateTime = 0.35f;
    private PlayerLocamotion parent;
    public IdelState(PlayerFSM fsm, CharactorBaseState parent) : base(fsm)
    {
        this.fsm = fsm;
        this.parent = parent as PlayerLocamotion;
    }

    public override void OnEnter()
    {
        if (animator == null)
            animator = fsm.GetAnimator();

        if(animator.GetFloat("Movement") == 0)
            animator.CrossFadeInFixedTime("Idle", translateTime);

        fsm.PlayerInput.moveDir += MoveAction;
    }

    public override void OnExit()
    {
        animator.SetFloat("Movement", 0);
        fsm.PlayerInput.moveDir -= MoveAction;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {

    }
    private void MoveAction(Vector2 inputVec2)
    {
        if(inputVec2 != Vector2.zero)
        {
            parent.ChangeState(PlayerLoacmotionState.Walk);
        }
    }

}
