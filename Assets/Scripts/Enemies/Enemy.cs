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
            StartCoroutine(Death());
            anim.SetTrigger("triggerDead");
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        CheckHealth();
        anim.SetTrigger("triggerDamaged");
        StartCoroutine(ApplyDamageTexture());
    }

    private IEnumerator ApplyDamageTexture()
    {
        rendererRef.material.SetColor("_EmissionColor", new Color(0.3f, 0f, 0f));
            rendererRef.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.5f);
            rendererRef.material.DisableKeyword("_EMISSION");
    }

    private IEnumerator Death()
    {
        yield return null;
    }
}
