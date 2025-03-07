using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class DefendState : CharactorBaseState
    {
        private NormalStates parent;
        private new HumanFSM fsm;
        private float currentTime = 0f;
        public DefendState(FSM fsm, CharactorBaseState parent) : base(fsm)
        {
            this.fsm = (fsm as HumanFSM);
            this.parent = parent as NormalStates;
        }

        public override void OnEnter()
        {
            animator.CrossFadeInFixedTime("DefendStart", fsm.humanData.normalData.crossTime_Defend);
            animator.SetBool("IsDefend",true);
            fsm.humanData.runTimeData.canHitBack = false;
            fsm.humanData.runTimeData.isInvincible = true;
            currentTime = 0f;
        }

        public override void OnExit()
        {
            fsm.humanData.runTimeData.isInvincible = false;
            fsm.humanData.runTimeData.canHitBack = true;
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            currentTime += Time.deltaTime;

            if (fsm.humanData.runTimeData.currentPower < fsm.humanData.normalData.defendCostPower)
            {
                parent.ChangeState(NormalState.None);
            }

            if ((currentTime > fsm.humanData.normalData.allContineTime_Defend))
            {
                animator.SetBool("IsDefend", false);
            }

            if (currentTime > fsm.humanData.normalData.allContineTime_Defend + fsm.humanData.normalData.defendendTime)
            {
                parent.ChangeState(NormalState.None);
            }
        }
    }
}
