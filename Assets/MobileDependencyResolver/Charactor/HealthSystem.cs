using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private float maxHealth = 100f;
    private float currentHealth = 0;

    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        currentHealth = maxHealth;
    }
    public void Demage(Transform attacker,float hit)
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        currentHealth -= hit;
        GetComponent<Rigidbody>().AddForce(-(attacker.position - transform.position) + Vector3.up * 5f);
    }
}
