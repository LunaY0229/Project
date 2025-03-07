using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : FSM<MasterState>
{
    public MonsterFSMData monsterData;
    private Vector3 hitVec;
    private bool IsCreate = false;

    public float Health = 100f;

    private void Start()
    {
        states.Add(MasterState.Locamotion,new MonsterLocamotion(this));
        states.Add(MasterState.Skills, new MonsterSkillStates(this));
        states.Add(MasterState.Hit, new MonsterHitState(this));
        

        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        yield return new WaitForSeconds(1.2f);
        IsCreate = true;
    }

    bool isCreate = false;

    protected void Update()
    {
        if(Vector3.Distance(GameObject.FindWithTag("Player").transform.position,transform.position) < 5f && isCreate == false)
        {
            isCreate = true;
            ChangState(MasterState.Locamotion);
        }

        if (IsCreate == false) return;

        if(states.ContainsKey(currentState))
            states[currentState].OnUpdate();
    }

    public override void Demage(Transform attacker, float hit, float force)
    {
        base.Demage(attacker, hit, force);

        monsterData.datas.power -= hit * monsterData.datas.powerCostMulti;

        if(monsterData.datas.power <= 0)
        {
            monsterData.datas.canHitBack = true;
            transform.Find("HD").gameObject.SetActive(false);
        }

        Health -= hit;

        if (Health <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (monsterData.datas.canHitBack)
        {
            (states[MasterState.Hit] as MonsterHitState).SetAttacker(attacker);
            ChangState(MasterState.Hit);
        }
    }

    private void OnDrawGizmos()
    {
        var skillIndex = 0;
        var pos = transform.position + monsterData.skillDatas[skillIndex].SkillCheckForwardOffset *transform.forward + monsterData.skillDatas[skillIndex].SkillCheckUpOffset * Vector3.up + monsterData.skillDatas[skillIndex].SkillCheckRightOffset * transform.right;
        var radius = monsterData.skillDatas[skillIndex].SkillCheckRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pos,radius);
    }
}

public enum MasterState
{
    None,
    Locamotion,
    Attack,
    Hit,
    Skills
}
