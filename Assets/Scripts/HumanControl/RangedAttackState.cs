using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay.HumanFSM
{
    public class RangedAttackState : BaseSubState<RangedAttackType>
    {
        public BaseSubState parent;
        public RangedAttackState(FSM fsm,BaseSubState parent) : base(fsm)
        {
            this.parent = parent;
            allChildState.Add(RangedAttackType.Combo_1, new RangedAttack_1(fsm, this));
            (fsm as HumanFSM).humanData.runTimeData.rangedAttackList[0].isCD = false;
        }

        public override void OnEnter()
        {
            ChangeState(RangedAttackType.Combo_1);
        }

        public override void OnExit()
        {
            ChangeState(RangedAttackType.None);
        }
    }

    public enum RangedAttackType
    {
        None,
        Combo_1,
    }
}
