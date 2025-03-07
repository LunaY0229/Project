using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MonsterHitState : BaseSubState
{
    float currentTime = 0;
    float hitMoveSpeed = 0f;
    Transform attacker;
    Vector3 hitVec;

    public void SetAttacker(Transform attacker)
    {
        this.attacker = attacker;
    }

    public MonsterHitState(FSM fsm) : base(fsm)
    {

    }

    public override void OnEnter()
    {
        currentTime = 0;

        // 获取被攻击者的方向（根据被攻击者的朝向）
        Vector3 attackerPosition = attacker.position;
        Vector3 targetPosition = fsm.transform.position; // 当前对象的transform

        // 计算攻击者相对于被攻击者的位置
        Vector3 directionToAttacker = attackerPosition - targetPosition;

        animator.SetFloat("HitRight", directionToAttacker.x > 0 ? 1 : -1);
        animator.SetFloat("HitForward", directionToAttacker.z > 0 ? 1 : -1);

        hitVec = -directionToAttacker;
        hitVec.y = 0;

        animator.CrossFadeInFixedTime("Hit", 0.3f);

        hitMoveSpeed = (fsm as MonsterFSM).monsterData.hitData.hitMoveSpeed;
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
        hitMoveSpeed -= Time.deltaTime;

        if (hitMoveSpeed < 0) hitMoveSpeed = 0;

        fsm.transform.position += hitVec * Time.deltaTime * hitMoveSpeed;

        if (currentTime > (fsm as MonsterFSM).monsterData.hitData.hitTime)
        {
            (fsm as MonsterFSM).ChangState(MasterState.Locamotion);
        }
    }
}
