using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterRunStates : CharactorBaseState
{
    // ��Ҫ�������
    private readonly NavMeshAgent _agent;
    private readonly Transform _playerTransform;

    // ���ò���
    private const float AttackRange = 3.5f;    // ������������
    private const float UpdateInterval = 0.2f; // Ŀ����¼�����Ż����ܣ�
    private float _lastUpdateTime;

    public MonsterRunStates(FSM fsm, BaseSubState parent) : base(fsm)
    {
        // ��ȡ�������
        _agent = fsm.GetComponent<NavMeshAgent>();

        // ��ȡ������ã����������"Player"��ǩ��
        var player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // ����NavMeshAgent����
        if (_agent != null)
        {
            _agent.speed = 5f;       // �ƶ��ٶ�
            _agent.angularSpeed = 360; // ��ת�ٶ�
            _agent.stoppingDistance = AttackRange * 0.8f; // �����������
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

        // �����벢�л�״̬
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