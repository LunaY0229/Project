using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class NormalStates : BaseSubState<NormalState>
    {

        public NormalStates(FSM fsm) : base(fsm)
        {
            allChildState.Add(NormalState.JumpBack, new JumpBack(fsm, this));
            allChildState.Add(NormalState.Defend, new DefendState(fsm, this));
            allChildState.Add(NormalState.RangedAttackState, new RangedAttackState(fsm, this));
            allChildState.Add(NormalState.LooktargetBackWalk, new LookTargetBackWalkState(fsm, this));
            allChildState.Add(NormalState.NormalAttack, new NormalAttackState(fsm, this));
        }

        public override void OnEnter()
        {
            Think();
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {
            if (currentState == NormalState.None)
            {
                Think();
            }
            else
            {
                base.OnUpdate();
            }

            if ((fsm as HumanFSM).humanData.runTimeData.target != null)
            {
                var target = (fsm as HumanFSM).humanData.runTimeData.target.position;
                target.y = fsm.transform.position.y;
                fsm.transform.LookAt(target);
            }
        }

        private void Think()
        {
            var myFSM = (fsm as HumanFSM);

            if (myFSM.humanData.runTimeData.target != null)
            {
                bool canDoRange = myFSM.IsAnyRangedAttackCanDo();
                bool canDoNormal = myFSM.IsAnyNormalAttackCanDo();
                float targetDis = myFSM.GetDistance();

                if (targetDis < myFSM.humanData.normalData.toTargetDisBlow_JumpBack && (myFSM.humanData.runTimeData.currentPower < myFSM.humanData.normalData.humanEscapePowerNum && myFSM.humanData.runTimeData.canEscape) && (!canDoRange && !canDoNormal))
                {
                    //能量不足，优先撤退恢复能量
                    ChangeState(NormalState.JumpBack);
                }
                //行为全在cd尝试防御
                else if (targetDis < 3f && (!canDoRange && !canDoNormal) && myFSM.humanData.runTimeData.currentPower >= myFSM.humanData.normalData.defendCostPower)
                {
                    ChangeState(NormalState.Defend);
                }
                else if ((!canDoRange && !canDoNormal))
                {
                    ChangeState(NormalState.LooktargetBackWalk);
                }
                else if(canDoRange && !canDoNormal && myFSM.humanData.runTimeData.canEscape)
                {
                    ChangeState(NormalState.JumpBack);
                }
                //能量充足，并且有技能可用，根据距离判断采用远或近的攻击
                else if (targetDis > myFSM.humanData.normalData.chooseAttackNum && canDoRange)
                {
                    ChangeState(NormalState.RangedAttackState);
                }
                else if (targetDis < myFSM.humanData.normalData.chooseAttackNum && canDoNormal)
                {
                    ChangeState(NormalState.NormalAttack);
                }
                else if(targetDis > myFSM.humanData.normalData.chooseAttackNum)
                {
                    animator.SetFloat("Speed", 2f);
                    animator.SetFloat("Right", 0f);
                }
            }
        }
    }

    public enum NormalState
    {
        None,
        JumpBack,
        Defend,
        LooktargetBackWalk,
        NormalAttack,
        RangedAttackState
    }
}
