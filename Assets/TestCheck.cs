using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCheck : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<FSM>().Demage(transform, 10, 5f);
            GameObjectPool.Instance.PushObj(transform.parent.gameObject);
        }
    }
}
