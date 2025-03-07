using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="¹ÖÎïÊý¾Ý",fileName = "new Data")]
public class MonsterFSMData : ScriptableObject
{
    public RunTimeData datas;
    public List<SkillData> skillDatas;
    public HitaData hitData;
}
[Serializable]
public class RunTimeData
{
    public bool canHitBack = true;
    public float power = 100f;
    public float powerCostMulti = 0.2f;
}

[Serializable]
public class HitaData
{
    public float hitTime = 0.5f;
    public float hitMoveSpeed = 2f;
}

[Serializable]
public class SkillData
{
    public float weight = 1f;
    public string skillName = "";
    public float crossTime = 0.15f;

    public bool CanPlay = true;
    public float SkillCD = 0f;
    public float SkillBefore = 0.2f;
    public float SkillLast = 0.5f;
    public float SkillAllTime = 3f;
    public AudioClip startClip;

    public float PlayEffectTime = 1f;

    public bool HasDeamge = true;
    public bool IsCheckDeamge = false;
    public float demageTime = 0f;
    public float SkillCheckRadius = 5f;
    public float SkillCheckForwardOffset = 1f;
    public float SkillCheckRightOffset = 0f;
    public float SkillCheckUpOffset = 0f;
    public float DeamgeNum = 0f;
    public float DemageForce = 0f;
}
