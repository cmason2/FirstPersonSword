using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Renderer rendererRef;
    Animator anim;
    [SerializeField] int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

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

    void CheckHealth()
    {
        if (hp <= 0)
        {
            anim.SetTrigger("triggerDead");
            StartCoroutine(Death());
        }
    }

    public void TakeDamage(int damage)
    {
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

    private IEnumerator Death()
    {
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<MoveEnemyToPlayer>());
        yield return new WaitForSeconds(1f);
        Destroy(GetComponent<BoxCollider>());
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.3f;
        collider.center = new Vector3(0f, 0.25f, 0f);
        //BoxCollider collider = GetComponent<BoxCollider>();
        //collider.size = new Vector3(collider.size.x,0.5f,collider.size.z);
        //collider.center = new Vector3(collider.center.x, 0.25f, collider.center.z);
        gameObject.name = "Dead Spider";
        gameObject.layer = 7;

    }
}
