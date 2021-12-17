using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatEnemy : Enemy
{
    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        CheckHealth();
        StartCoroutine(ApplyDamageEffect());
    }

    protected override void Death()
    {
        StartCoroutine(DeathActions());
    }

    private IEnumerator DeathActions()
    {
        Destroy(GetComponent<BatMovement>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<Animation>());
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddTorque(transform.right * 100f);
        rb.AddForce(FindObjectOfType<Player>().transform.forward * 1000f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
