using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : Enemy
{
    Renderer[] renderersRef;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        renderersRef = GetComponentsInChildren<Renderer>();
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        CheckHealth();
        StartCoroutine(ApplyDamageEffect());
    }

    private IEnumerator ApplyDamageEffect()
    {
        foreach (Renderer renderer in renderersRef)
        {
            if(renderer != null)
            {
                renderer.material.SetColor("_EmissionColor", new Color(0.3f, 0f, 0f));
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
        yield return new WaitForSeconds(0.5f);
        foreach (Renderer renderer in renderersRef)
        {
            if(renderer != null)
                renderer.material.DisableKeyword("_EMISSION");
        }
    }
}
