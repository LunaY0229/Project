using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Skill_3 : MonsterSkillBase
{
    Vector3 playerTrans;
    public Skill_3(FSM fsm, BaseSubState parent) : base(fsm, parent)
    {
        skillIndex = 2;
    }

    public override void OnEnter()
    {
        fsm.GetComponent<NavMeshAgent>().enabled = false;
        base.OnEnter();
        playerTrans = GameObject.FindWithTag("Player").transform.position;
    }

    protected override void Effect()
    {
        
    }
}
