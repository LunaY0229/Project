using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MonsterSkillStates : BaseSubState<MonsterSkillType>
{
    MonsterFSMData data;
    private Coroutine cooldownCoroutine;
    private bool isInCooldownBehavior;
    private Transform targetPlayer;

    public MonsterSkillStates(FSM fsm) : base(fsm)
    {
        allChildState.Add(MonsterSkillType.Skill_1, new Skill_1(fsm, this));
        allChildState.Add(MonsterSkillType.Skill_2, new Skill_2(fsm, this));
        allChildState.Add(MonsterSkillType.Skill_3, new Skill_3(fsm, this));
        data = (fsm as MonsterFSM).monsterData;
    }

    public override void OnEnter()
    {
        targetPlayer = GameObject.FindWithTag("Player").transform;

        TryFindAvailableSkill(out int index);

        if (index != -1)
        {
            StartSkill(index);
        }
        else
        {
            currentState = MonsterSkillType.None;
            StartCooldownBehavior();
        }
    }

    public override void OnUpdate()
    {
        if(currentState != MonsterSkillType.None)
        {
            base.OnUpdate();
        }

        if (isInCooldownBehavior)
        {
            // 持续检查技能可用性
            TryFindAvailableSkill(out int index);
            if (index != -1)
            {
                InterruptCooldownBehavior();
                StartSkill(index);
            }
        }
    }

    public override void OnExit()
    {
        InterruptCooldownBehavior();
        animator.speed = 1f;
    }

    private void TryFindAvailableSkill(out int foundIndex)
    {
        foundIndex = -1;
        float maxWeight = 0f;

        for (int i = 0; i < data.skillDatas.Count; i++)
        {
            if (data.skillDatas[i].CanPlay && data.skillDatas[i].weight > maxWeight)
            {
                maxWeight = data.skillDatas[i].weight;
                foundIndex = i;
            }
        }
    }

    private void StartSkill(int index)
    {
        fsm.StartCoroutine(EnterCD(data.skillDatas[index]));
        ChangeState((MonsterSkillType)(index + 1));
    }

    private void StartCooldownBehavior()
    {
        isInCooldownBehavior = true;
        cooldownCoroutine = fsm.StartCoroutine(CooldownBehavior());
    }

    private void InterruptCooldownBehavior()
    {
        if (cooldownCoroutine != null)
        {
            fsm.StopCoroutine(cooldownCoroutine);
            cooldownCoroutine = null;
        }
        isInCooldownBehavior = false;
        animator.speed = 1f;
    }

    private IEnumerator CooldownBehavior()
    {
        float moveSpeed = 1f;
        float rotationSpeed = 2f;
        float sideStepInterval = 4f;
        float retreatProbability = 0.15f;

        Vector3 basePosition = fsm.transform.position;
        float direction = 1f;

        while (true)
        {
            // 面向玩家
            Vector3 toPlayer = targetPlayer.position - fsm.transform.position;
            toPlayer.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(toPlayer);
            fsm.transform.rotation = Quaternion.Slerp(
                fsm.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // 左右踱步移动
            Vector3 lateralMovement = fsm.transform.right * direction * moveSpeed * Time.deltaTime;
            animator.SetFloat("Right", direction, 0.15f, Time.deltaTime);
            animator.speed = 0.3f;

            fsm.transform.position += lateralMovement;

            // 间隔改变方向
            sideStepInterval -= Time.deltaTime;
            if (sideStepInterval <= 0)
            {
                direction *= -1;
                sideStepInterval = UnityEngine.Random.Range(5f, 10f);

                // 30%概率后退
                if (UnityEngine.Random.value < retreatProbability)
                {
                    Vector3 retreatMovement = -fsm.transform.forward * moveSpeed * 1.5f * Time.deltaTime;
                    fsm.transform.position += retreatMovement;
                }
            }

            // 保持在一定范围内移动
            if (Vector3.Distance(fsm.transform.position, basePosition) > 5f)
            {
                direction *= -1;
                basePosition = fsm.transform.position;
            }

            yield return null;
        }
    }

    IEnumerator EnterCD(SkillData data)
    {
        data.CanPlay = false;
        yield return new WaitForSecondsRealtime(data.SkillCD);
        data.CanPlay = true;
    }
}

public enum MonsterSkillType
{
    None,
    Skill_1,
    Skill_2,
    Skill_3,
}

