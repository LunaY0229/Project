using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class BaseCharactorFSM<T> : FSM<T> where T : Enum
{
    [Header("=================Settings======================")]
    [SerializeField, Header("��ֱ�ٶ�")] protected float verticalSpeed = 0;
    [SerializeField, Header("����")] protected float characterGravity = -9;
    [SerializeField, Header("��ЩlayerΪ����")] LayerMask whatIsGround;
    [SerializeField, Header("������뾶")] private float GroundDetectionRadius = 0.19f;
    [SerializeField, Header("����������ƫ��")] private float GroundDetectionOffset = -0.08f;
    [SerializeField, Header("����ʱ��")] private float fallOutdeltaTimer = 0f;
    [SerializeField, Header("�����˳�ʱ��")] private float fallOutTimer = 0.2f;
    [SerializeField, Header("��������ٶ�")] private float maxVerticalSpeed = 20f;
    [SerializeField, Header("��С�����ٶ�")] private float minVerticalSpeed = -10f;
    [SerializeField,Header("���������߳���")]private float SlopDetectionLenth = 1f;

    public bool isHasGravity = true;

    public PlayerCharactorData data;


    private Vector3 verticalVelocity;
    private Vector3 groundDetectionOrigin;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        GroundDetecion();
        UpdateChracterGravity();
        UpDateVerticalVelocity();
        UpDateVerticalVelocity();
    }

    public void AddForce(float verticalSpeed)
    {
        this.verticalSpeed = verticalSpeed;
    }

    /// <summary>
    /// ����
    /// </summary>
    protected void UpdateChracterGravity()
    {
        if (isOnGround)
        {
            fallOutdeltaTimer = fallOutTimer;

            if (verticalSpeed < 0)
            {
                verticalSpeed = -2;
            }
        }
        else
        {
            if (isHasGravity)
            {
                if (fallOutdeltaTimer >= 0)
                {
                    fallOutdeltaTimer -= Time.deltaTime;
                }
                else
                {
                    if (verticalSpeed < maxVerticalSpeed && verticalSpeed > minVerticalSpeed)
                    {
                        verticalSpeed += characterGravity * Time.deltaTime;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ���´�ֱ�ٶ�
    /// </summary>
    protected void UpDateVerticalVelocity()
    {
        verticalVelocity.Set(0, verticalSpeed, 0);
        characterController.Move(verticalVelocity * Time.deltaTime);

    }
    /*
    /// <summary>
    /// ���¶�����λ��
    /// </summary>
    /// 
    protected virtual void OnAnimatorMove()
    {
        //animator.ApplyBuiltinRootMotion();
        //UpdateCharacterVelocity(animator.deltaPosition);
    }*/

    protected virtual void UpdateCharacterVelocity(Vector3 movement)
    {
        Vector3 dir = ResetVelocityOnSlop(movement);
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
        {
            characterController.Move(dir * Time.deltaTime * data.moveMult);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
        {
            characterController.Move(dir * Time.deltaTime * data.dodgeMult);
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="characterVelosity"></param>
    protected Vector3 ResetVelocityOnSlop(Vector3 characterVelosity)
    {

        if (Physics.Raycast(transform.position, Vector3.down, out var groundHit, SlopDetectionLenth, whatIsGround))
        {
            float newAngle = Vector3.Dot(Vector3.up, groundHit.normal);
            //�����컨��ͽ�ɫ��������ʱ
            if (newAngle != -1 && verticalSpeed <= 0)
            {
                return Vector3.ProjectOnPlane(characterVelosity, groundHit.normal);
            }
        }
        return characterVelosity;

    }


    /// <summary>
    /// ������
    /// </summary>
    protected void GroundDetecion()
    {
        groundDetectionOrigin = new Vector3(transform.position.x, transform.position.y - GroundDetectionOffset, transform.position.z);
        isOnGround = Physics.CheckSphere(groundDetectionOrigin, GroundDetectionRadius, whatIsGround, QueryTriggerInteraction.Ignore);
    }

    private void OnDrawGizmos()
    {
        if (isOnGround)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - GroundDetectionOffset, transform.position.z), GroundDetectionRadius);
    }

    public Material cyMaterial;

    /// <summary>
    /// ������Ӱ
    /// </summary>
    public void PlayCY()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        for(int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            GameObject gObj = new GameObject();

            gObj.transform.SetLocalPositionAndRotation(transform.position, skinnedMeshRenderers[i].transform.rotation);

            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            MeshFilter mf =  gObj.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();

            skinnedMeshRenderers[i].BakeMesh(mesh);
            mf.mesh = mesh;
            mr.material = cyMaterial;

            
            Destroy(gObj, 1.5f);
            StartCoroutine(ChangeAlpha(mr.material));
        }
    }

    IEnumerator ChangeAlpha(Material mesh)
    {
        float value = mesh.GetFloat("_Alpha");
        while(value > 0)
        {
            value -= Time.deltaTime;
            mesh.SetFloat("_Alpha",value);
            yield return null;
        }
    }

    public void CountTime(float time, Action callBack)
    {
        StartCoroutine(CoCounTime(time,callBack));
    }

    IEnumerator CoCounTime(float time, Action callBack)
    {
        yield return new WaitForSeconds(time);
        callBack?.Invoke();
    }
}
