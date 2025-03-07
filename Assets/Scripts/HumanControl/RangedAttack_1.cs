using GamePlay.HumanFSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class RangedAttack_1 : CharactorBaseState
    {
        HumanFSM humanFSM;
        RangedAttackState parent;
        private float currentTime;
        private float exitTime = 0;
        public RangedAttack_1(FSM fsm, RangedAttackState normalStates) : base(fsm)
        {
            humanFSM = fsm as HumanFSM;
            parent = normalStates;
        }

        public override void OnEnter()
        {
            animator.CrossFadeInFixedTime("RangedAttack_1", humanFSM.humanData.runTimeData.rangedAttackList[0].crossTime);
            currentTime = 0f;
            exitTime = ReturnAnimatorTimer(animator, "RangedAttack_1");
            humanFSM.humanData.runTimeData.rangedAttackList[0].isCD = true;
            humanFSM.CountTime(humanFSM.humanData.runTimeData.rangedAttackList[0].cd,() =>
            {
                humanFSM.humanData.runTimeData.rangedAttackList[0].isCD = false;
            });

            if (humanFSM.humanData.runTimeData.rangedAttackList[0].startClip != null)
            {
                SourcesManager.Instance.PlayOnShot(humanFSM.humanData.runTimeData.rangedAttackList[0].startClip,1f);
            }
        }

        public override void OnExit()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {
            currentTime += Time.deltaTime;

            if (currentTime > exitTime + humanFSM.humanData.runTimeData.rangedAttackList[0].exitTime)
            {
                parent.ChangeState(RangedAttackType.None);
                (parent.parent as NormalStates).ChangeState(NormalState.None);
                return;
            }
        }

        private float ReturnAnimatorTimer(Animator animator, string animator_Name)
        {
            float length = 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals(animator_Name))
                {
                    length = clip.length;
                    break;
                }
            }
            return length;
        }
    }
}
