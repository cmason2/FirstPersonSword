using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Renderer[] renderersRef;
    [SerializeField] protected int hp;
    public bool isInvulnerable = false;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    protected virtual void Start()
    {
        renderersRef = GetComponentsInChildren<Renderer>();
    }

    protected void CheckHealth()
    {
        if (hp <= 0)
        {
            Death();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        Debug.Log(gameObject.name + " took " + damage + " damage!");
        hp -= damage;
        CheckHealth();
    }

    public void SetInvulnerability(bool value)
    {
        isInvulnerable = value;
        if(isInvulnerable)
        {
            ApplyInvulnerabilityEffect();
        }
        else
        {
            RemoveInvulnerabilityEffect();
        }
    }

    private void ApplyInvulnerabilityEffect()
    {
        foreach (Renderer renderer in renderersRef)
        {
            if (renderer != null)
            {
                renderer.material.SetColor("_EmissionColor", new Color(0.3f, 0f, 0.3f));
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
    }

    private void RemoveInvulnerabilityEffect()
    {
        foreach (Renderer renderer in renderersRef)
        {
            if (renderer != null)
                renderer.material.DisableKeyword("_EMISSION");
        }
    }

    protected IEnumerator ApplyDamageEffect()
    {
        foreach (Renderer renderer in renderersRef)
        {
            if (renderer != null)
            {
                renderer.material.SetColor("_EmissionColor", new Color(0.3f, 0f, 0f));
                renderer.material.EnableKeyword("_EMISSION");
            }
        }
        yield return new WaitForSeconds(0.5f);
        foreach (Renderer renderer in renderersRef)
        {
            if (renderer != null)
                renderer.material.DisableKeyword("_EMISSION");
        }
    }

    protected virtual void Death()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ParticleSystem>().Stop();
    }
}
