using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Skill_1 : MonsterSkillBase
{
    private float currenTime = 0f;
    public Skill_1(FSM fsm, BaseSubState parent) : base(fsm, parent)
    {
        skillIndex = 0;
        monsterData.skillDatas[skillIndex].CanPlay = true;

    }

    protected override void Effect()
    {
        base.Effect();

        if (monsterData.skillDatas[skillIndex].HasDeamge)
        {
            DemageEnemy();
            monsterData.skillDatas[skillIndex].IsCheckDeamge = true;
        }
    }
}
