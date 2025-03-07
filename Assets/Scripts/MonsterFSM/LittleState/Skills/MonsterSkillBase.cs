using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MonsterSkillBase : CharactorBaseState
{
    protected int skillIndex = 0;
    protected new MonsterFSM fsm;
    protected MonsterFSMData monsterData;
    private float currentTime = 0;
    protected BaseSubState parent;
    private bool isPlay;
    Vector3 playerPos;
    public MonsterSkillBase(FSM fsm,BaseSubState parent) : base(fsm)
    {
        this.fsm = fsm as MonsterFSM;
        this.parent = parent;
        monsterData = this.fsm.monsterData;
    }

    public override void OnEnter()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;

        animator.CrossFade(monsterData.skillDatas[skillIndex].skillName, monsterData.skillDatas[skillIndex].crossTime);
        currentTime = 0;
        isPlay = false;

        if(monsterData.skillDatas[skillIndex].startClip != null)
        {
            SourcesManager.Instance.PlayOnShot(monsterData.skillDatas[skillIndex].startClip,1f);
        }
    }

    public override void OnExit()
    {
        monsterData.skillDatas[skillIndex].IsCheckDeamge = false;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        UpdateRotation(playerPos, 60);
        currentTime += Time.deltaTime;

        if(!isPlay && currentTime > monsterData.skillDatas[skillIndex].PlayEffectTime)
        {
            isPlay = true;
            Effect();
        }

        if (currentTime >= monsterData.skillDatas[skillIndex].SkillAllTime + monsterData.skillDatas[skillIndex].SkillLast)
        {
            fsm.ChangState(MasterState.Locamotion);
        }
    }


    protected virtual void Effect()
    {

    }

    private void UpdateRotation(Vector3 target, float timer)
    {
        var direction = (target - fsm.transform.position).normalized;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        fsm.transform.rotation = Quaternion.Slerp(fsm.transform.rotation, lookRotation, UnTetheredLerp(timer));
    }

    public float UnTetheredLerp(float time = 10f)
    {
        return 1 - Mathf.Exp(-time * Time.deltaTime);
    }

    protected void DemageEnemy()
    {
        var pos = fsm.transform.position + monsterData.skillDatas[skillIndex].SkillCheckForwardOffset * fsm.transform.forward + monsterData.skillDatas[skillIndex].SkillCheckUpOffset * Vector3.up + monsterData.skillDatas[skillIndex].SkillCheckRightOffset * fsm.transform.right;

        var radius = monsterData.skillDatas[skillIndex].SkillCheckRadius;

        // 使用 OverlapSphere 来检测攻击范围内的敌人
        Collider[] enemiesInRange = Physics.OverlapSphere(pos, radius);

        foreach (var enemy in enemiesInRange)
        {
            // 判断敌人是否符合攻击条件
            if (enemy.CompareTag("Player"))
            {
                if (enemy.gameObject != fsm.gameObject)
                {
                    enemy.GetComponent<FSM>().Demage(fsm.transform, monsterData.skillDatas[skillIndex].DeamgeNum, monsterData.skillDatas[skillIndex].DemageForce);
                }
            }
        }
    }
}
