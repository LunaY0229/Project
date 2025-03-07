using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBaseState
{
    protected FSM fsm;

    public EntityBaseState(FSM fsm)
    {
        this.fsm = fsm;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnFixedUpdate();
    public abstract void OnExit();

    public virtual void OnGizemos() { }

    protected void PushObj(GameObject obj, float time)
    {
        fsm.StartCoroutine(CoPush(obj, time));
    }

    IEnumerator CoPush(GameObject obj,float time)
    {
        yield return new WaitForSeconds(time);
        GameObjectPool.Instance.PushObj(obj);
    } 
}
