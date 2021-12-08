using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    public static int bossHealth = 1;

    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        BossHealthBar.currentHealth = hp;//new
        CheckHealth();
        StartCoroutine(ApplyDamageEffect());
    }

    protected override void Death()
    {
        bossHealth = 0;
        Destroy(GameObject.Find("BossHealthBar"));
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        gameObject.GetComponentInChildren<Animation>().Stop();
        Destroy(gameObject.GetComponent<BossMovement>());
        gameObject.AddComponent<Rigidbody>().AddTorque(transform.right * 100f);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
