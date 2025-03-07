using UnityEngine;

public class PlayerHitState : CharactorBaseState
{
    PlayerFSM player;
    float currentTime = 0;
    bool canStop = false;
    float hitMoveSpeed = 0f;
    Transform attacker;
    Vector3 hitVec;

    public PlayerHitState(PlayerFSM fsm) : base(fsm)
    {
        player = fsm;
        
    }

    public override void OnEnter()
    {
        currentTime = 0;
        canStop = false;
        animator.CrossFadeInFixedTime("BackHit",0.35f);
        SourcesManager.Instance.PlayOnShot(player.data.playerHitData.hitclip[Random.Range(0, player.data.playerHitData.hitclip.Count)], 1f);

        // 获取被攻击者的方向（根据被攻击者的朝向）
        Vector3 attackerPosition = attacker.position;
        Vector3 targetPosition = fsm.transform.position; // 当前对象的transform

        // 计算攻击者相对于被攻击者的位置
        Vector3 directionToAttacker = attackerPosition - targetPosition;

        hitVec = directionToAttacker.normalized;

        player.PlayerInput.dodgeAction += DodgeAction;
    }

    private void DodgeAction()
    {
        if ((fsm as PlayerFSM).data.playerDodgeData.canDodge)
        {
            (fsm as PlayerFSM).data.playerDodgeData.canDodge = false;
            (fsm as PlayerFSM).CountTime((fsm as PlayerFSM).data.playerDodgeData.dodgeCD, () => { (fsm as PlayerFSM).data.playerDodgeData.canDodge = true; });
            (fsm as PlayerFSM).ChangState(PlayerState.Roll);
        }

    }

    public void SetAttacker(Transform attacker,float force)
    {
        this.attacker = attacker;
        hitMoveSpeed = force;
    }

    public override void OnExit()
    {
        player.PlayerInput.dodgeAction -= DodgeAction;
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        hitMoveSpeed -= Time.deltaTime;

        if (hitMoveSpeed < 0) hitMoveSpeed = 0;

        fsm.transform.forward = hitVec;

        fsm.transform.position += -fsm.transform.forward * Time.deltaTime * hitMoveSpeed;

        currentTime += Time.deltaTime;

        if(currentTime > player.data.playerHitData.canDodgeTime)
        {
            canStop = true;
        }

        if(currentTime >= player.data.playerHitData.AnimationTime)
        {
            player.ChangState(PlayerState.Locamotion);
        }
    }
}
