using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class LookTargetBackWalkState : CharactorBaseState
    {
        private NormalStates parent;
        private float playerDis;
        private HumanFSM humanFSM;
        private float currentTime = 0f;
        private float time = 0;
        private bool dirRight;

        public LookTargetBackWalkState(FSM fsm,BaseSubState parent) : base(fsm)
        {
            this.parent = parent as NormalStates;
            humanFSM = fsm as HumanFSM;
        }

        public override void OnEnter()
        {
            if (humanFSM.humanData.runTimeData.target == null)
            {
                parent.ChangeState(NormalState.None);
                return;
            }
            currentTime = 0f;
            dirRight = UnityEngine.Random.Range(0, 100f) > 40f;
            time = UnityEngine.Random.Range(humanFSM.humanData.normalData.backCheckTimeMin, humanFSM.humanData.normalData.backCheckTimeMax);
            playerDis = humanFSM.GetDistance();
        }

        public override void OnExit()
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("Right", 0);
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            if(humanFSM.humanData.runTimeData.target == null)
            {
                parent.ChangeState(NormalState.None);
                return;
            }

            if(humanFSM.GetDistance() < playerDis && humanFSM.GetDistance() < humanFSM.humanData.normalData.backCheckDis)
            {
                if (humanFSM.IsAnyNormalAttackCanDo())
                {
                    if (UnityEngine.Random.Range(0, 100) < 40)
                    {
                        parent.ChangeState(NormalState.NormalAttack);
                        return;
                    }
                    else
                    {
                        parent.ChangeState(NormalState.Defend);
                        return;
                    }
                }
                else
                {
                    parent.ChangeState(NormalState.Defend);
                }
            }

            currentTime += Time.deltaTime;

            if (currentTime > time)
            {
                animator.SetFloat("Speed", 0, 0.15f, Time.deltaTime);
                animator.SetFloat("Right", 0, 0.15f, Time.deltaTime);
            }
            else
            {
                if (humanFSM.GetDistance() > humanFSM.humanData.normalData.backCheckDisMax)
                {
                    animator.SetFloat("Speed", 2, 0.15f, Time.deltaTime);
                    animator.SetFloat("Right", 0, 0.15f, Time.deltaTime);
                }
                else if (humanFSM.GetDistance() < humanFSM.humanData.normalData.backCheckDisMin)
                {
                    animator.SetFloat("Speed", -1, 0.15f, Time.deltaTime);
                    animator.SetFloat("Right", 0, 0.15f, Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Speed", 0, 0.15f, Time.deltaTime);
                    animator.SetFloat("Right", dirRight ? 1 : -1, 0.15f, Time.deltaTime);
                }
            }

            if (currentTime > time + 0.15f)
            {
                parent.ChangeState(NormalState.None);
                return;
            }
        }
    }
}
