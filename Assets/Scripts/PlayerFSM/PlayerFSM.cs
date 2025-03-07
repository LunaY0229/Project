using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerFSM : BaseCharactorFSM<PlayerState>
{
    public Text PlayerNameText;
    public string playerName;

    /// <summary>
    /// 输入
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// 组件
    /// </summary>
    private PlayerDatas playerDatas = new PlayerDatas();
    public PlayerInput PlayerInput { get { return playerInput; } }
    public PlayerDatas PlayerDatas { get { return playerDatas; } }
    public Animator GetAnimator() { return animator; }

    public Transform firePos;
    public Slider healthSlider;
    public Text healthText;

    public GameObject diePanel;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        states.Add(PlayerState.Locamotion,new PlayerLocamotion(this));
        states.Add(PlayerState.Attck,new PlayerComboState(this));
        states.Add(PlayerState.Roll, new DodgeState(this));
        states.Add(PlayerState.Hit, new PlayerHitState(this));


        data.playerQualitydata.currentHealth = data.playerQualitydata.maxHealth;

        if (playerInput != null)
            ChangState(PlayerState.Locamotion);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void FixedUpdate()
    {
        if (playerInput == null) return;

        states[currentState].OnFixedUpdate();
    }


    protected override void Update()
    {
        healthText.text = data.playerQualitydata.currentHealth + "/" + data.playerQualitydata.maxHealth;
        healthSlider.value = data.playerQualitydata.currentHealth / data.playerQualitydata.maxHealth;

        base.Update();

        if (playerInput == null) return;

        states[currentState].OnUpdate();
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        PlayerNameText.text = name;
    }

    public void PlayFootSound()
    {

    }

    public void PlayFootBackSound()
    {

    }

    public override void Demage(Transform attacker, float hit, float force)
    {
        Debug.Log("收到攻击");
        if (data.playerDodgeData.isPerfecting)
        {
            SourcesManager.Instance.PlayOnShot(data.playerDodgeData.perfectdodgeClip[UnityEngine.Random.Range(0, data.playerDodgeData.perfectdodgeClip.Count)],1f);
            PF(data.playerDodgeData.stopTimeNum, data.playerDodgeData.resetTimeSpeed);
            PlayCY();
            return;
        }

        if (currentState == PlayerState.Roll) return;

        data.playerQualitydata.currentHealth -= hit;

        if(data.playerQualitydata.currentHealth < 0)
        {
            diePanel.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        (states[PlayerState.Hit] as PlayerHitState).SetAttacker(attacker,force);

        ChangState(PlayerState.Hit);
    }

    private void OnDrawGizmos()
    {
        if(states.ContainsKey(currentState))
            states[currentState].OnGizemos();
    }

    public void PF(float time,float speed)
    {
        StartCoroutine(CoPF(time,speed));
    }

    IEnumerator CoPF(float time, float speed)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(time);

        var t = 0f;
        while (t < 1)
        {
            // 关键修改：使用真实时间增量
            t += Time.unscaledDeltaTime * speed;
            t = Mathf.Clamp01(t); // 确保不超界

            Time.timeScale = t;

            // 优化等待方式
            yield return null; // 每帧执行一次
        }

        Time.timeScale = 1; // 确保最终恢复
    }

    public void PlayWeaponBackSound()
    {

    }
}

[Serializable]
public class PlayerDatas
{
    [Header("=====Locamotion====")]
    public float moveSpeed = 7.5f;
    public float runSpeed = 5f;
    public float turnSpeed = 5f;
    public float rotationSpeed = 500f;

    [Header("")]
    public float acctackIntervalbefore = 0.1f;
    public float acctackTime = 0.2f;
}

public enum PlayerConBo
{
    None,
    NormalAttack
}

public enum PlayerLoacmotionState
{
    None,
    Idel,
    Run,
    Walk
}

public enum PlayerState
{
    None,
    Locamotion,
    Jump,
    Attck,
    Roll,
    Hit
}
