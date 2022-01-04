using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript_Mine : EnemyScript_Base
{
    [SerializeField] private float impactDamage = 20.0f;
    [SerializeField] private float impactRepulse = 50.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("TakeDamage", impactDamage, SendMessageOptions.DontRequireReceiver);
            Vector3 forceVector = (collision.transform.position - transform.position).normalized * impactRepulse;
            forceVector = new Vector3(forceVector.x, forceVector.y/10.0f, forceVector.z);
            collision.gameObject.GetComponent<PlayerController>().ApplyForce(forceVector);
            DestroyEnemy();
        }
    }
}
