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

        // ��ȡ�������Ļ�ϵ�λ��
        Vector3 mousePosition = UnityEngine.Input.mousePosition;

        // �������ߴ�����������λ��
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // ��������棨���������Collider���ཻ�ĵ�
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // ��ȡ���߻��е�λ��
            Vector3 targetPosition = hit.point;

            pos = hit.point;

            // ����Ŀ��λ�ú����λ�õķ���
            Vector3 direction = targetPosition - fsm.transform.position;

            // ������ת�Ƕ� (ֻ����Y����ת��������3D)
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            var targetRo = Quaternion.Euler(0, angle, 0);
            // ����ҵ���ת����Ϊ�������߻��е�λ��
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

        // ����ǰ�������ƶ�����������귽��
        Vector3 moveDirection = new Vector3(dir.x, 0f, dir.z); // ������x, z������ƶ�

        // �ƶ����
        fsm.GetComponent<Rigidbody>().MovePosition(fsm.transform.position + moveDirection * Time.deltaTime * fsm.PlayerDatas.moveSpeed);

        // ����Animator������ǰ���ٶ�
        fsm.GetAnimator().SetFloat("Forward", vector.y, 0.3f, Time.deltaTime);
    }

    private void AttackUp()
    {
        isExit = true;
        fsm.GetAnimator().SetBool("Attack", false);
    }
}
