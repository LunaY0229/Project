using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    public class JumpBack : CharactorBaseState
    {
        private NormalStates parent;
        private new HumanFSM fsm;
        private float currentTime = 0f;
        public JumpBack(FSM fsm,CharactorBaseState parent) : base(fsm)
        {
            this.fsm = (fsm as HumanFSM);
            this.parent = parent as NormalStates;
            this.fsm.humanData.runTimeData.canEscape = true;
        }

        public override void OnEnter()
        {
            currentTime = 0f;
            fsm.humanData.runTimeData.canEscape = false;
            animator.CrossFadeInFixedTime("DodgeBack", fsm.humanData.normalData.crossTime_JumpBack);
            fsm.CountTime(fsm.humanData.normalData.escapeCD,() => { fsm.humanData.runTimeData.canEscape = true; });
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

            if(currentTime > fsm.humanData.normalData.allContineTime_JumpBack)
            {
                parent.ChangeState(NormalState.LooktargetBackWalk);
            }
        }
    }
}
