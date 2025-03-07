using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GamePlay.HumanFSM
{
    public class Combo_2 : ComboBase
    {
        public Combo_2(FSM fsm, NormalAttackState normalStates) : base(fsm, normalStates)
        {
        }

        public override void OnEnter()
        {
            comboIndex = 1;
            base.OnEnter();
        }
    }
}
