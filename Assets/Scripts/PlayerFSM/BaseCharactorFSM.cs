using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class BaseCharactorFSM<T> : FSM<T> where T : Enum
{
    [Header("=================Settings======================")]
    [SerializeField, Header("垂直速度")] protected float verticalSpeed = 0;
    [SerializeField, Header("重力")] protected float characterGravity = -9;
    [SerializeField, Header("哪些layer为地面")] LayerMask whatIsGround;
    [SerializeField, Header("地面检测半径")] private float GroundDetectionRadius = 0.19f;
    [SerializeField, Header("地面检测向下偏移")] private float GroundDetectionOffset = -0.08f;
    [SerializeField, Header("下落时间")] private float fallOutdeltaTimer = 0f;
    [SerializeField, Header("下落退出时间")] private float fallOutTimer = 0.2f;
    [SerializeField, Header("最大下落速度")] private float maxVerticalSpeed = 20f;
    [SerializeField, Header("最小下落速度")] private float minVerticalSpeed = -10f;
    [SerializeField,Header("地面检测射线长度")]private float SlopDetectionLenth = 1f;

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
    /// 重力
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
    /// 更新垂直速度
    /// </summary>
    protected void UpDateVerticalVelocity()
    {
        verticalVelocity.Set(0, verticalSpeed, 0);
        characterController.Move(verticalVelocity * Time.deltaTime);

    }
    /*
    /// <summary>
    /// 更新动画根位移
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
    /// 坡面检测
    /// </summary>
    /// <param name="characterVelosity"></param>
    protected Vector3 ResetVelocityOnSlop(Vector3 characterVelosity)
    {

        if (Physics.Raycast(transform.position, Vector3.down, out var groundHit, SlopDetectionLenth, whatIsGround))
        {
            float newAngle = Vector3.Dot(Vector3.up, groundHit.normal);
            //不在天花板和角色在坡面上时
            if (newAngle != -1 && verticalSpeed <= 0)
            {
                return Vector3.ProjectOnPlane(characterVelosity, groundHit.normal);
            }
        }
        return characterVelosity;

    }


    /// <summary>
    /// 地面检测
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
    /// 创建残影
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
