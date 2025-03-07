using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    public string id;
    protected Vector3 targetPos;

    public void Init(Vector3 pos,string id)
    {
        this.id = id;
        targetPos = pos;        
    }


    public void Update()
    {
        RowMove();
    }

    protected abstract void RowMove();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HealthSystem>(out HealthSystem sys))
        {
            sys.Demage(transform, 10f);
        }
        GameObjectPool.Instance.PushObj(gameObject);
    }
}
