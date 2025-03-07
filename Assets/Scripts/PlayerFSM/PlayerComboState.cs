using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboState : BaseSubState<PlayerConBo>
{
    PlayerInput input;
    public PlayerComboState(PlayerFSM fsm) : base(fsm)
    {
        input = fsm.PlayerInput;
        allChildState.Add(PlayerConBo.NormalAttack,new NormalAttack(fsm,this));
    }

    public override void OnEnter()
    {
        input.dodgeAction += Dodge;

        if (currentState == PlayerConBo.None)
            ChangeState(PlayerConBo.NormalAttack);
    }

    public override void OnExit()
    {
        input.dodgeAction -= Dodge;

        ChangeState(PlayerConBo.None);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void Dodge()
    {
        if ((fsm as PlayerFSM).data.playerAttackData.canExit)
        {
            if ((fsm as PlayerFSM).data.playerDodgeData.canDodge)
            {
                (fsm as PlayerFSM).data.playerDodgeData.canDodge = false;
                (fsm as PlayerFSM).CountTime((fsm as PlayerFSM).data.playerDodgeData.dodgeCD, () => {(fsm as PlayerFSM).data.playerDodgeData.canDodge = true; });
                (fsm as PlayerFSM).ChangState(PlayerState.Roll);
            }
        }
    }
}
