using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : BaseSubState
{
    private new PlayerFSM fsm;
    private Vector2 dir;
    private string Dodge_Back = "Dodge_Back";
    private string Dodge_Forward = "Dodge_Front";
    private float currentTime = 0.1f;
    private float crossTime = 0.11f;
    private float animationTime = 0.4f;
    private bool isPlayering = false;
    private bool isSetting = false;

    public DodgeState(PlayerFSM fsm) : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void OnEnter()
    {
        fsm.data.playerDodgeData.isPerfecting = true;

        fsm.PlayerInput.moveDir += MoveAction;

        currentTime = 0.1f;
        animationTime = fsm.data.playerDodgeData.animationTime;
        isPlayering = false;
        isSetting = false;
        animator.SetBool("HasInput", false);
        SourcesManager.Instance.PlayOnShot(fsm.data.playerDodgeData.dodgeClip[UnityEngine.Random.Range(0, fsm.data.playerDodgeData.dodgeClip.Count)], 1f);
    }

    public override void OnExit()
    {
        fsm.PlayerInput.moveDir -= MoveAction;
    }

    public override void OnFixedUpdate()
    {
        
    }
    float currentVelocity;
    public override void OnUpdate()
    {
        currentTime -= Time.deltaTime;
        animationTime -= Time.deltaTime;

        if((fsm.data.playerDodgeData.animationTime - animationTime) >= fsm.data.playerDodgeData.perfectDodgeTime)
        {
            fsm.data.playerDodgeData.isPerfecting = false;
        }

        if (currentTime <= 0 && isPlayering == false)
        {
            isPlayering = true;

            if (dir == Vector2.zero)
                animator.CrossFadeInFixedTime(Dodge_Back, crossTime);
            else
            {
                var targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

                fsm.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(fsm.transform.eulerAngles.y, targetAngle, ref currentVelocity,0);
                animator.CrossFadeInFixedTime(Dodge_Forward, crossTime);
            }
        }

        if (animationTime <= 0)
        {
            animator.SetBool("HasInput", true);

            if (dir == Vector2.zero)
            {
                animator.SetFloat("Movement", 0);
                fsm.ChangState(PlayerState.Locamotion);
            }
            else
                fsm.ChangState(PlayerState.Locamotion, Enum.GetName(typeof(PlayerLoacmotionState), PlayerLoacmotionState.Run));
        }
    }

    private void MoveAction(Vector2 inputVec2)
    {
        dir = inputVec2;
    }
}
