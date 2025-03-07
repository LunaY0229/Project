using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : CharactorBaseState
{
    protected new PlayerFSM fsm;
    private float translateTime = 0.3f;
    private PlayerLocamotion parent;
    private string animatorName = "Movement";
    public WalkState(PlayerFSM fsm, CharactorBaseState parent) : base(fsm)
    {
        this.fsm = fsm;
        this.parent = parent as PlayerLocamotion;
    }

    public override void OnEnter()
    {
        if (animator == null)
            animator = fsm.GetAnimator();

        animator.CrossFadeInFixedTime("WalkStart", translateTime);
        animator.SetBool("HasInput", true);

        fsm.PlayerInput.moveDir += MoveAction;
    }

    public override void OnExit()
    {
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
        if (inputVec2 == Vector2.zero)
        {
            parent.ChangeState(PlayerLoacmotionState.Idel);
            animator.SetBool("HasInput", false) ;
            return;
        }
        animator.SetFloat(animatorName, inputVec2.sqrMagnitude * 2, 0.35f, Time.deltaTime);
    }
}
