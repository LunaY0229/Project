using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class ComboBase : CharactorBaseState
    {
        HumanFSM humanFSM;
        NormalAttackState parent;
        protected int comboIndex = 0;
        int currentCombo = 0;
        float currenTime = 0f;
        float exitTime = 0f;

        public ComboBase(FSM fsm,NormalAttackState normalStates) : base(fsm)
        {
            humanFSM = fsm as HumanFSM;
            parent = normalStates;
        }

        public override void OnEnter()
        {
            animator.SetFloat("Speed", 0);
            currenTime = 0f;
            currentCombo = 0;
            exitTime = ReturnAnimatorTimer(animator, "Combo_" + (comboIndex + 1) +"_" + currentCombo) - 0.15f;
            animator.CrossFadeInFixedTime("Combo_" + (comboIndex + 1) + "_" + currentCombo, humanFSM.humanData.runTimeData.normalAttackList[comboIndex].crossTime);
            animator.SetBool("HasAttackInput", true);
            humanFSM.humanData.runTimeData.currentPower -= humanFSM.humanData.runTimeData.normalAttackList[currentCombo].costPower;
            UpdateRotation();

            humanFSM.humanData.runTimeData.normalAttackList[comboIndex].isCD = true;

            humanFSM.CountTime(humanFSM.humanData.runTimeData.normalAttackList[comboIndex].cd, () =>
            {
                humanFSM.humanData.runTimeData.normalAttackList[comboIndex].isCD = false;
            });

            if (humanFSM.humanData.runTimeData.normalAttackList[comboIndex].startClip != null)
            {
                SourcesManager.Instance.PlayOnShot(humanFSM.humanData.runTimeData.normalAttackList[comboIndex].startClip, 1f);
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
            currenTime += Time.deltaTime;

            if (currenTime > exitTime + humanFSM.humanData.runTimeData.normalAttackList[comboIndex].exitTime)
            {
                parent.ChangeState(NormalAttackType.None);
                (parent.parent as NormalStates).ChangeState(NormalState.None);
                return;
            }

            if (currenTime > exitTime)
            {
                Think();
                return;
            }
             

        }

        private void Think()
        {
            if (currentCombo < humanFSM.humanData.runTimeData.normalAttackList[comboIndex].maxCombo - 1)
            {
                if (humanFSM.GetDistance() < humanFSM.humanData.runTimeData.normalAttackList[comboIndex].exitDis[currentCombo])
                {
                    currentCombo++;
                    UpdateRotation();
                    currenTime = 0;
                    exitTime = ReturnAnimatorTimer(animator, "Combo_" + (comboIndex + 1) + "_" + currentCombo) - 0.15f;
                }
                else
                {
                    animator.SetBool("HasAttackInput", false);
                }
            }
            else
            {
                animator.SetBool("HasAttackInput", false);
            }
        }
        

        private void UpdateRotation()
        {
            var target = (fsm as HumanFSM).humanData.runTimeData.target.position;
            target.y = fsm.transform.position.y;
            fsm.transform.LookAt(target);
        }

        /// <summary>
        /// 获取animator的时间，注意是Animator!!!!
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animator_Name">动画的名字</param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取animation的时间，注意是Animation!!!!
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="animation_Name">动画的名字</param>
        /// <returns></returns>
        private float ReturnAnimationTimer(Animation animation, string animation_Name)
        {
            float length = 0;
            AnimationClip animationClip = animation.GetClip(animation_Name);
            length = animationClip.length;
            return length;
        }
    }
}
