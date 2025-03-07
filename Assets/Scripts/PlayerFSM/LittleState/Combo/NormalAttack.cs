using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class NormalAttack : CharactorBaseState
{
    private new PlayerFSM fsm;
    private int attackIndex = 0;
    private float currentTime = 0;
    private float exitTime = 0;
    private AttackData attackData;
    private List<float> playVFXTime = new List<float>();
    private Transform fsvContain;
    private Transform attackTransform;
    private GameObject currenttargetEnemy;
    private int countVfx = 0;
    float currentVelocity;

    public NormalAttack(PlayerFSM fsm,CharactorBaseState parent) : base(fsm)
    {
        this.fsm = fsm;
        fsvContain = fsm.transform.Find("FVXContain");
    }

    public override void OnEnter()
    {
        attackIndex = 0;
        currentTime = 0;
        exitTime = 0;
        countVfx = 0;
        fsm.PlayerInput.attackActionDown += AttackAction;
        playVFXTime.Clear();
        PlayAttackAnimationByIndex();
        animator.SetBool("HasInput", true);
    }

    public override void OnExit()
    {
        fsm.PlayerInput.attackActionDown -= AttackAction;
        animator.SetBool("HasInput", false);
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        if (currenttargetEnemy != null)
        {
            UpdateRotation(currenttargetEnemy.transform.position,60);
        }

        if (!fsm.data.playerAttackData.canAttack)
        {
            currentTime += Time.deltaTime;

            if(playVFXTime.Count > 0 && currentTime >= playVFXTime[0])
            {
                attackTransform = fsvContain.Find((((attackIndex - 1 + fsm.data.playerAttackData.allAttack.Count) % fsm.data.playerAttackData.allAttack.Count)) + "_" + countVfx);
                attackTransform.GetChild(0).gameObject.SetActive(true);
                fsm.StartCoroutine(WaitDe(attackTransform.GetChild(0).gameObject));
                CameraShake(attackData.AttackSharkeCameraForce);

                if(Random.Range(0,100) < 10)
                {
                    SourcesManager.Instance.PlayOnShot(attackData.vedioClip[Random.Range(0, attackData.vedioClip.Count)], 1f);
                }

                SourcesManager.Instance.PlayOnShot(attackData.attackClip[Random.Range(0, attackData.attackClip.Count)],1f);

                if (DemageEnemy())
                {
                    SourcesManager.Instance.PlayOnShot(attackData.hitClip[Random.Range(0, attackData.hitClip.Count)], 1f);
                    PF(attackData.StopTime);
                }
                playVFXTime.RemoveAt(0);
                countVfx++;
            }

            if(currentTime >= attackData.AttackAllTime + attackData.AttackLast)
            {
                fsm.data.playerAttackData.canAttack = true;
                currentTime = 0;
            }

            if(currentTime > attackData.AttackAllTime)
            {
                fsm.data.playerAttackData.canExit = true;
            }
        }
        else
        {
            exitTime += Time.deltaTime;

            if(exitTime >= fsm.data.playerAttackData.resetAttackIndexTime)
            {
                fsm.ChangState(PlayerState.Locamotion);
            }
        }
    }

    private void AttackAction()
    {
        if (fsm.data.playerAttackData.canAttack)
        {
            PlayAttackAnimationByIndex();
        }
    }

    private void PlayAttackAnimationByIndex()
    {
        attackData = fsm.data.playerAttackData.allAttack[attackIndex];
        animator.CrossFadeInFixedTime(attackData.AttackName, attackData.crossTime);
        GameObjectPool.GetInstance().GetGameObjectByName(attackData.vfxName);
        fsm.data.playerAttackData.canAttack = false;
        fsm.data.playerAttackData.canExit = false;
        attackIndex++;
        attackIndex %= fsm.data.playerAttackData.allAttack.Count; ;
        exitTime = 0;
        countVfx = 0;
        currenttargetEnemy = CheckEnemy();


        for (int i = 0; i < attackData.playVFXTime.Count; i++)
        {
            playVFXTime.Add(attackData.playVFXTime[i]);
        }
    }

    private GameObject CheckEnemy()
    {
        // 获取攻击的位置，攻击范围中心
        var pos = fsm.transform.position + attackData.AttackForwardOffect * fsm.transform.forward;
        var radius = attackData.AttackRadius;

        // 使用 OverlapSphere 来检测攻击范围内的敌人
        Collider[] enemiesInRange = Physics.OverlapSphere(pos, radius);

        foreach (var enemy in enemiesInRange)
        {
            // 判断敌人是否符合攻击条件
            if (enemy.TryGetComponent<FSM>(out FSM enemyFSM))
            {
                if (enemy.gameObject != fsm.gameObject)
                    return enemy.gameObject;
            }
        }

        return null;
    }

    private bool DemageEnemy()
    {
        bool isAttack = false;
        // 获取攻击的位置，攻击范围中心
        var pos = fsm.transform.position + attackData.AttackForwardOffect * fsm.transform.forward + attackData.AttackUpOffect * Vector3.up;
        var radius = attackData.AttackRadius;

        // 使用 OverlapSphere 来检测攻击范围内的敌人
        Collider[] enemiesInRange = Physics.OverlapSphere(pos, radius);

        foreach (var enemy in enemiesInRange)
        {
            // 判断敌人是否符合攻击条件
            if (enemy.TryGetComponent<FSM>(out FSM enemyFSM))
            {
                if (enemy.gameObject != fsm.gameObject)
                {
                    var poss = enemy.transform.position;
                    var dir = enemy.transform.position - fsm.transform.position;
                    poss = (poss + dir * 0.1f);
                    poss.y = fsm.transform.position.y + attackData.AttackUpOffect;
                    var hitVFX = GameObject.Instantiate(Resources.Load<GameObject>(attackData.hitVfxName), poss, Quaternion.identity);
                    isAttack = true;
                    enemyFSM.Demage(fsm.transform, attackData.AttackDemage,attackData.AttackForce);
                }
            }
        }
        return isAttack;
    }

    IEnumerator WaitDe(GameObject obj)
    {
        yield return new WaitForSeconds(0.75f);
        obj.SetActive(false);
    }

    public override void OnGizemos()
    {
        base.OnGizemos();

        if (attackData == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(fsm.transform.position + attackData.AttackForwardOffect * fsm.transform.forward, attackData.AttackRadius);
    }


    private void UpdateRotation(Vector3 target,float timer)
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

    public void CameraShake(float shakeForce)
    {
        if (shakeForce == 0) { return; }

        var cinemachineImpulseSource = GameObject.Find("CameraShark").GetComponent<CinemachineImpulseSource>();
        cinemachineImpulseSource.GenerateImpulseWithForce(shakeForce);
    }

    Coroutine PauseFrameCoroutine;
    Animator currentEnemyAnimator;
    Animator currentCharacterAnimator;

    public void PF(float time)
    {
        if (time == 0) { Debug.Log("顿帧时间为0退出"); return; }
        if (currenttargetEnemy == null) return;

        currentEnemyAnimator = currenttargetEnemy.GetComponent<Animator>();
        currentCharacterAnimator = fsm.GetAnimator();

        if (currentCharacterAnimator == null)
        {
            Debug.Log("currentCharacterAnimator is null!");
            return;
        }
        if (currentEnemyAnimator == null)
        {
            Debug.Log("currentEnemyAnimator is null!");
            return;
        }

        if (PauseFrameCoroutine != null)
        {
            fsm.StopCoroutine(PauseFrameCoroutine);
        }
        PauseFrameCoroutine = fsm.StartCoroutine(PauseFrameOnAnimation(time));
    }

    IEnumerator PauseFrameOnAnimation(float time)
    {
        Debug.LogWarning("进入顿帧协程" + time);
        currentCharacterAnimator.speed = 0f;
        currentEnemyAnimator.speed = 0f;
        //VFXManager.MainInstance.PauseVFX();
        yield return new WaitForSeconds(time);
        //VFXManager.MainInstance.ResetVXF();
        currentCharacterAnimator.speed = 1f;

        if(currentEnemyAnimator != null)
            currentEnemyAnimator.speed = 1f;
    }
}
