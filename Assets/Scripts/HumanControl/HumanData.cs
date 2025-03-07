using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePlay.HumanFSM
{
    [CreateAssetMenu(menuName = "人类",fileName = "newPeople")]
    public class HumanData : ScriptableObject
    {
        public RunTimeData runTimeData;
        public HumanNormalData normalData;
    }

    [Serializable]
    public class RunTimeData
    {
        [Header("是否霸体")] public bool canHitBack;
        [Header("是否无敌")] public bool isInvincible = false;

        public Transform target;

        public bool canEscape = true; 

        public float currentHealth = 100f;
        public float currentPower = 100f;

        public List<AttackData> rangedAttackList;
        public List<AttackData> normalAttackList;
    }

    [Serializable]
    public class AttackData
    {
        public bool isCD = false;
        public float radius = 2f;
        public float costPower = 5f;
        public float crossTime = 0.35f;
        public int maxCombo = 4;
        public float exitTime = 0.2f;
        public float cd = 4f;

        public List<float> force;
        public List<float> comboTime;
        public List<float> exitDis;
        public List<float> attackCheckTime;
        public AudioClip startClip;
        public List<AudioClip> actionTimeClip;
    }

    [Serializable]
    public class HumanNormalData
    {
        public float maxPowerNum = 200f;

        [Header("npc能量获取的速度")]public float powerGetSpeed = 5f;
        [Header("逃离玩家的能量值阈值")] public float humanEscapePowerNum = 10f;

        [Header("后跳相关")]
        public float escapeCD = 10f;
        public float toTargetDisBlow_JumpBack = 4f;
        public float crossTime_JumpBack = 0.3f;
        public float allContineTime_JumpBack = 1.1f;

        public float chooseAttackNum = 5f;
        [Header("后退检测玩家距离发起攻击距离")]public float backCheckDis = 3f;
        [Header("后退检测玩家最大距离")]public float backCheckDisMax = 7f;
        [Header("后退检测玩家最小距离")]public float backCheckDisMin = 5f;
        [Header("后退恢复体力的随机最大值")]public float backCheckTimeMin = 2f;
        [Header("后退恢复体力的随机最小值")]public float backCheckTimeMax = 5f;

        [Header("防御")]
        public float crossTime_Defend = 0.35f;
        public float allContineTime_Defend = 2f;
        public float defendCostPower = 2f;
        public float defendendTime = 0.2f;

    }
}
