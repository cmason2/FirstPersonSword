using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderEnemy : Enemy
{
    Renderer rendererRef;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rendererRef = GetComponentInChildren<Renderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        CheckHealth();
        anim.SetTrigger("triggerDamaged");
        StartCoroutine(ApplyDamageEffect());
    }

    private IEnumerator ApplyDamageEffect()
    {
        rendererRef.material.SetColor("_EmissionColor", new Color(0.3f, 0f, 0f));
            rendererRef.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.5f);
            rendererRef.material.DisableKeyword("_EMISSION");
    }

    protected override void Death()
    {
        StartCoroutine(DeathActions());
    }

    private IEnumerator DeathActions()
    {
        anim.SetTrigger("triggerDead");
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<MoveEnemyToPlayer>());
        yield return new WaitForSeconds(1f);
        Destroy(GetComponent<BoxCollider>());
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.3f;
        collider.center = new Vector3(0f, 0.25f, 0f);
        gameObject.AddComponent<Rigidbody>().angularDrag = 3f;
        //BoxCollider collider = GetComponent<BoxCollider>();
        //collider.size = new Vector3(collider.size.x,0.5f,collider.size.z);
        //collider.center = new Vector3(collider.center.x, 0.25f, collider.center.z);
        gameObject.name = "Dead Spider";
        gameObject.layer = 7;

    }
}
