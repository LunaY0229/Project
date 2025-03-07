using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlay.HumanFSM
{
    public class Combo_1 : ComboBase
    {
        public Combo_1(FSM fsm, NormalAttackState normalStates) : base(fsm, normalStates)
        {
        }

        public override void OnEnter()
        {
            comboIndex = 0;
            base.OnEnter();
        }
    }
}
