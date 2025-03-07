using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewCharactorData", fileName = "NewCharactorData")]
public class PlayerCharactorData : ScriptableObject
{
    [Range(0f, 100), SerializeField, Header("移动位移倍率")] public float moveMult = 0.2f;
    [Range(0f, 100), SerializeField, Header("移动位移倍率副本")] public float moveMult_Origin = 0.2f;
    [Range(0f, 60), SerializeField, Header("闪避位移倍率")] public float dodgeMult = 0.2f;
    [Range(0f, 60), SerializeField, Header("闪避位移倍率副本")] public float dodgeMult_Origin = 0.2f;

    public PlayerCharactorAttackData playerAttackData;
    public PlayerCharactorHitData playerHitData;
    public PlayerDodgeData playerDodgeData;
    public PlayerQuality playerQualitydata;
}

[Serializable]
public class PlayerQuality
{
    public float currentHealth;
    public float maxHealth;
}

[Serializable]
public class PlayerCharactorHitData 
{
    public float AnimationTime = 1f;
    public float Hitmult = 0.4f;
    public float Hitmult_Origin = 0.4f;
    public float canDodgeTime = 0.1f;
    public List<AudioClip> hitclip;
}


[Serializable]
public class PlayerCharactorLocamotionData
{

}

[Serializable]
public class PlayerDodgeData
{
    public bool canDodge = false;
    public float animationTime = 0.7f;
    public float stopTimeNum = 0.15f;
    public float resetTimeSpeed = 3f;
    public bool isPerfecting;
    public float perfectDodgeTime = 0.2f;
    public float dodgeCD = 0.35f;
    public List<AudioClip> dodgeClip;
    public List<AudioClip> perfectdodgeClip;
}

[Serializable]
public class PlayerCharactorAttackData
{
    public float resetAttackIndexTime = 0.3f;
    public bool canExit = false;
    public bool canAttack = true;
    public List<AttackData> allAttack;
}
[Serializable]
public class AttackData
{
    public string AttackName = "";
    public string vfxName = "";
    public string hitVfxName = "FX_hit_04";
    public float crossTime = 0.2f;
    public float AttackBefore = 0.1f;
    public float AttackCheckOffect = 0.3f;
    public float AttackSharkeCameraForce = 0.1f;
    public float AttackLast = 0.1f;
    public float AttackAllTime = 1f;
    public float AttackForwardOffect = 1f;
    public float AttackUpOffect = 1f;
    public float AttackRadius = 2f;
    public float StopTime = 0.1f;
    public float AttackForce = 0.1f;
    public float AttackDemage = 10f;
    public List<float> playVFXTime;

    public List<AudioClip> hitClip;
    public List<AudioClip> attackClip;
    public List<AudioClip> vedioClip;
}
