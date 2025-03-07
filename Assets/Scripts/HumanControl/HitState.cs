using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.HumanControl
{
    public class HitState : BaseSubState
    {
        float hitMoveSpeed = 0f;
        Transform attacker;
        Vector3 hitVec;
        public void SetAttacker(Transform attacker, float force)
        {
            this.attacker = attacker;
            hitMoveSpeed = force;
        }

        public HitState(FSM fsm) : base(fsm)
        {

        }

        public override void OnEnter()
        {
            // 获取被攻击者的方向（根据被攻击者的朝向）
            Vector3 attackerPosition = attacker.position;
            Vector3 targetPosition = fsm.transform.position; // 当前对象的transform

            // 计算攻击者相对于被攻击者的位置
            Vector3 directionToAttacker = attackerPosition - targetPosition;

            hitVec = -directionToAttacker;
        }

        public override void OnExit()
        {
            
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override void OnUpdate()
        {
            hitMoveSpeed -= Time.deltaTime * 5f;

            if (hitMoveSpeed < 0) hitMoveSpeed = 0;

            fsm.transform.position += hitVec * Time.deltaTime * hitMoveSpeed;
        }
    }
}
