using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocamotion : BaseSubState<PlayerLoacmotionState>
{
    public new PlayerFSM fsm;
    private float currentVelocity;
    public PlayerLocamotion(PlayerFSM fsm) : base(fsm)
    {
        this.fsm = fsm;
        allChildState.Add(PlayerLoacmotionState.Idel, new IdelState(fsm, this));
        allChildState.Add(PlayerLoacmotionState.Walk, new WalkState(fsm, this));
        allChildState.Add(PlayerLoacmotionState.Run, new RunState(fsm, this));
    }

    public override void OnEnter()
    {
        fsm.PlayerInput.moveDir += UpdateRotation;
        fsm.PlayerInput.dodgeAction += DodgeAction;
        fsm.PlayerInput.attackActionDown += AttackAction;

        if(currentState == PlayerLoacmotionState.None)
            ChangeState(PlayerLoacmotionState.Idel);
    }

    public override void OnExit()
    {
        fsm.PlayerInput.moveDir -= UpdateRotation;
        fsm.PlayerInput.dodgeAction -= DodgeAction;
        fsm.PlayerInput.attackActionDown -= AttackAction;

        ChangeState(PlayerLoacmotionState.None);
    }

    public override void OnFixedUpdate()
    {
        base.OnUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void UpdateRotation(Vector2 inputDir)
    {
        if (inputDir == Vector2.zero) { return; }

        var targetAngle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        fsm.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(fsm.transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.08f);
    }

    private void DodgeAction()
    {
        if (fsm.data.playerDodgeData.canDodge)
        {
            fsm.data.playerDodgeData.canDodge = false;
            fsm.CountTime(fsm.data.playerDodgeData.dodgeCD,() => { fsm.data.playerDodgeData.canDodge = true; });
            fsm.ChangState(PlayerState.Roll);
        }
    }
    private void AttackAction()
    {
        fsm.ChangState(PlayerState.Attck);
    }
}
