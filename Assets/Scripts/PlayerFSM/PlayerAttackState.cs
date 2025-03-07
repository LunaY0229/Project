using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAttackState : CharactorBaseState
{
    protected new PlayerFSM fsm;
    GameObject VFX;
    bool isExit;
    float currentTime = 0;
    float currentAttackTime = 0;
    public PlayerAttackState(PlayerFSM fsm) : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void OnEnter()
    {
        fsm.GetAnimator().SetBool("Attack",true);
        fsm.PlayerInput.moveDir += PlayerMove;
        fsm.PlayerInput.attackActionUp += AttackUp;
        currentTime = 0;
        currentAttackTime = 0;
        isExit = false;
        isCreate = false;
        isCreateShoot = false;
    }

    public override void OnExit()
    {
        VFX.SetActive(false);
        fsm.PlayerInput.moveDir -= PlayerMove;
        fsm.PlayerInput.attackActionUp -= AttackUp;
    }

    public override void OnFixedUpdate()
    {

    }

    Vector3 pos;
    bool isCreate = false;
    bool isCreateShoot = false;

    public override void OnUpdate()
    {
        currentAttackTime += Time.deltaTime;

        if(currentAttackTime >= 0.8f && isCreate == false)
        {
            isCreate = true;
            VFX = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/VFX_Attack"), fsm.firePos);
        }

        if (isExit)
        {
            currentTime += Time.deltaTime;

            if (currentAttackTime > 1.5f && isCreateShoot == false)
            {
                isCreateShoot = true;

                var row = GameObjectPool.GetInstance().GetGameObjectByName(Config.ROW_NAME);
                row.transform.position = fsm.firePos.position;
                row.GetComponent<RowProjectile>().Init(pos, Config.GetRowIndex().ToString());

                
                var obj = GameObjectPool.GetInstance().GetGameObjectByName(Config.ROWSHOOT_NAME);
                obj.transform.position = fsm.firePos.position;
                PushObj(obj, 4f);
            }

            if(currentTime > 0.5f)
            {
                fsm.ChangState(PlayerState.Locamotion);
            }
        }

        // 获取鼠标在屏幕上的位置
        Vector3 mousePosition = UnityEngine.Input.mousePosition;

        // 发射射线从摄像机到鼠标位置
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // 射线与地面（假设地面有Collider）相交的点
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // 获取射线击中的位置
            Vector3 targetPosition = hit.point;

            pos = hit.point;

            // 计算目标位置和玩家位置的方向
            Vector3 direction = targetPosition - fsm.transform.position;

            // 计算旋转角度 (只考虑Y轴旋转，适用于3D)
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            var targetRo = Quaternion.Euler(0, angle, 0);
            // 将玩家的旋转设置为朝向射线击中的位置
            fsm.transform.rotation = targetRo;
        }
    }


    private void PlayerMove(Vector2 vector)
    {
        Vector3 dir = (vector.x * fsm.transform.right + vector.y * fsm.transform.forward);

        if (Vector3.Distance(fsm.transform.position, pos) < 2f && vector.y > 0)
        {
            fsm.GetAnimator().SetFloat("Forward", 0);
            return;
        }

        // 处理前后左右移动（独立于鼠标方向）
        Vector3 moveDirection = new Vector3(dir.x, 0f, dir.z); // 仅考虑x, z方向的移动

        // 移动玩家
        fsm.GetComponent<Rigidbody>().MovePosition(fsm.transform.position + moveDirection * Time.deltaTime * fsm.PlayerDatas.moveSpeed);

        // 更新Animator，设置前进速度
        fsm.GetAnimator().SetFloat("Forward", vector.y, 0.3f, Time.deltaTime);
    }

    private void AttackUp()
    {
        isExit = true;
        fsm.GetAnimator().SetBool("Attack", false);
    }
}
