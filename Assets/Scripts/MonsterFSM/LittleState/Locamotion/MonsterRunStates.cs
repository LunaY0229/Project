using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunStates : CharactorBaseState
{
    // 必要组件引用
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerTransform;

    // 配置参数
    private const float AttackRange = 3.5f;    // 攻击触发距离
    private const float UpdateInterval = 0.2f; // 目标更新间隔（优化性能）
    private float _lastUpdateTime;

    public MonsterRunStates(FSM fsm, BaseSubState parent) : base(fsm)
    {
        // 获取组件引用
        _agent = fsm.GetComponent<NavMeshAgent>();

        // 获取玩家引用（假设玩家有"Player"标签）
        var player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // 配置NavMeshAgent参数
        if (_agent != null)
        {
            _agent.speed = 5f;       // 移动速度
            _agent.angularSpeed = 360; // 旋转速度
            _agent.stoppingDistance = AttackRange * 0.8f; // 保留缓冲距离
        }
    }

    public override void OnEnter()
    {
        _agent.enabled = true;
    }

    public override void OnExit()
    {
        _agent.enabled = false;

        animator.SetFloat("Speed", 0);

    }

    public override void OnUpdate()
    {
        if (_playerTransform == null || _agent == null) return;

        animator.SetFloat("Speed", _agent.velocity.magnitude, 0.1f, Time.deltaTime);

        if (Time.time - _lastUpdateTime > UpdateInterval)
        {
            _agent.SetDestination(_playerTransform.position);
            _lastUpdateTime = Time.time;
        }

        // 检查距离并切换状态
        CheckDistanceToPlayer();
    }

    private void CheckDistanceToPlayer()
    {
        float distance = Vector3.Distance(
            fsm.transform.position,
            _playerTransform.position
        );

        if (distance <= AttackRange)
        {
            (fsm as MonsterFSM).ChangState(MasterState.Skills);
        }
    }

    public override void OnFixedUpdate()
    {

    }
}