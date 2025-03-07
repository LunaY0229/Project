using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Skill_2 : MonsterSkillBase
{
    public Skill_2(FSM fsm, BaseSubState parent) : base(fsm, parent)
    {
        skillIndex = 1;
        monsterData.skillDatas[skillIndex].CanPlay = true;
        this.fsm.monsterData.datas.canHitBack = false;
    }

    protected override void Effect()
    {
        base.Effect();

        fsm.transform.Find("HD").gameObject.SetActive(true);
        fsm.monsterData.datas.canHitBack = false;
        fsm.StartCoroutine(ChangeAlpha(fsm.transform.Find("HD").GetComponent<SkinnedMeshRenderer>().material));
        var obj = GameObjectPool.GetInstance().GetGameObjectByName("Bo");
        obj.transform.position = fsm.transform.position + Vector3.up * 3f + fsm.transform.forward;
        fsm.monsterData.datas.power = 100f;
        foreach (Transform child in obj.transform)
        {
            BlowAlpha(child.GetComponent<Renderer>().material);
        }

        PushObj(obj, 2);
    }

    IEnumerator ChangeAlpha(Material mesh)
    {
        float value = 0;
        while (value < 1)
        {
            value += Time.deltaTime;
            mesh.SetFloat("_Alpha", value);
            yield return null;
        }
        var cinemachineImpulseSource = GameObject.Find("CameraShark").GetComponent<CinemachineImpulseSource>();
        cinemachineImpulseSource.GenerateImpulseWithForce(0.4f);
    }

    IEnumerator BlowAlpha(Material mesh)
    {
        float value = mesh.GetFloat("_Alpha");
        while (value > 0)
        {
            value -= Time.deltaTime;
            mesh.SetFloat("_Alpha", value);
            yield return null;
        }
    }
}
