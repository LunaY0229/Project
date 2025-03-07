using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : CharactorBaseState
{
    protected new PlayerFSM fsm;
    private float translateTime = 0.15f;
    private PlayerLocamotion parent;
    private string animatorName = "Movement"; 
    public RunState(PlayerFSM fsm, CharactorBaseState parent) : base(fsm)
    {
        this.fsm = fsm;
        this.parent = parent as PlayerLocamotion;
    }

    public override void OnEnter()
    {
        if (animator == null)
            animator = fsm.GetAnimator();

        animator.SetBool("HasInput", true);
        animator.CrossFadeInFixedTime("Locomotion", translateTime);
        animator.SetFloat(animatorName, 3);

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
            animator.SetBool("HasInput", false);
            parent.ChangeState(PlayerLoacmotionState.Idel);
            return;
        }
        animator.SetFloat(animatorName, 3, translateTime,Time.deltaTime);
    }
}
