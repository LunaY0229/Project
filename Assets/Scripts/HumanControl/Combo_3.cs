using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GamePlay.HumanFSM
{
    public class Combo_3 : ComboBase
    {
        public Combo_3(FSM fsm, NormalAttackState normalStates) : base(fsm, normalStates)
        {

        }

        public override void OnEnter()
        {
            comboIndex = 2;
            base.OnEnter();
        }
    }
}
