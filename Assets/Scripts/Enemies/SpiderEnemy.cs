using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : Enemy
{
    Animator anim;
    SpiderMovement spiderMovement;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        spiderMovement = GetComponent<SpiderMovement>();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        CheckHealth();
        anim.SetTrigger("Damaged");
        StartCoroutine(ApplyDamageEffect());
        StartCoroutine(AttackCancel());
    }

    protected override void Death()
    {
        StartCoroutine(DeathActions());
    }

    private IEnumerator DeathActions()
    {
        anim.SetTrigger("Dead");
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<SpiderMovement>());
        yield return new WaitForSeconds(1f);
        Destroy(GetComponent<BoxCollider>());
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.3f;
        collider.center = new Vector3(0f, 0.25f, 0f);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.angularDrag = 3f;
        gameObject.name = "Dead Spider";
        gameObject.layer = 7;
    }

    private IEnumerator AttackCancel()
    {
        spiderMovement.attackCancelled = true;
        yield return new WaitForSeconds(1f);
        spiderMovement.attackCancelled = false;
    }
}
