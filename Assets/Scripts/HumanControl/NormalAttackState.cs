using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class NormalAttackState : BaseSubState<NormalAttackType>
    {
        public BaseSubState parent;
        public NormalAttackType attackBuffer = NormalAttackType.None;
        public NormalAttackState(FSM fsm,BaseSubState parent) : base(fsm)
        {
            this.parent = parent;
            allChildState.Add(NormalAttackType.Combo_1,new Combo_1(fsm,this));
            allChildState.Add(NormalAttackType.Combo_2,new Combo_2(fsm,this));
            allChildState.Add(NormalAttackType.Combo_3,new Combo_3(fsm,this));
            (fsm as HumanFSM).humanData.runTimeData.normalAttackList[0].isCD = false;
            (fsm as HumanFSM).humanData.runTimeData.normalAttackList[1].isCD = false;
            (fsm as HumanFSM).humanData.runTimeData.normalAttackList[2].isCD = false;
        }

        public override void OnEnter()
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("Right", 0);
            (fsm as HumanFSM).humanData.runTimeData.canHitBack = false;
            Think();
        }

        public override void OnUpdate()
        {
            if(currentState != NormalAttackType.None)
                base.OnUpdate();

            if (attackBuffer != NormalAttackType.None)
            {
                var attack = Enum.GetName(typeof(NormalAttackType), attackBuffer);
                attack = attack.Replace("Combo_","");
                var dis = (fsm as HumanFSM).humanData.runTimeData.normalAttackList[int.Parse(attack) - 1].radius;

                if((fsm as HumanFSM).GetDistance() < dis)
                {
                    ChangeState(attackBuffer);
                    attackBuffer = NormalAttackType.None;
                }
                else
                {
                    var target = (fsm as HumanFSM).humanData.runTimeData.target.position;
                    target.y = fsm.transform.position.y;
                    fsm.transform.LookAt(target);

                    animator.SetFloat("Speed",2f,0.35f,Time.deltaTime);
                }
            }
        }

        public override void OnExit()
        {
            (fsm as HumanFSM).humanData.runTimeData.canHitBack = true;
        }

        private void Think()
        {
            List<NormalAttackType> canDoType = new List<NormalAttackType>();

            if(!(fsm as HumanFSM).humanData.runTimeData.normalAttackList[0].isCD)
            {
                canDoType.Add(NormalAttackType.Combo_1);
            }

            if (!(fsm as HumanFSM).humanData.runTimeData.normalAttackList[1].isCD)
            {
                canDoType.Add(NormalAttackType.Combo_2);
            }

            if (!(fsm as HumanFSM).humanData.runTimeData.normalAttackList[2].isCD)
            {
                canDoType.Add(NormalAttackType.Combo_3);
            }

            attackBuffer = canDoType[UnityEngine.Random.Range(0, canDoType.Count)];
        }
    }

    public enum NormalAttackType
    {
        None,
        Combo_1,
        Combo_2,
        Combo_3,
    }
}
