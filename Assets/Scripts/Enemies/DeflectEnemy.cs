using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeflectEnemy : Enemy
{
    DeflectProjectile projectile;
    [SerializeField] ParticleSystem ps;
    Light light;
    Color deflectColor = new Color(0f, 1f, 0f);
    Color normalColor = new Color(1f, 0f, 0f);

    protected override void Start()
    {
        projectile = GetComponent<DeflectProjectile>();
        light = GetComponentInChildren<Light>();
    }

    public override void TakeDamage(int damage)
    {
        projectile.deflectNumber++;
        Debug.Log("Deflected: " + projectile.deflectNumber);
        if (projectile.deflectNumber % 2 == 1)
        {
            ps.startColor = deflectColor;
            light.color = deflectColor;
        }
        else
        {
            ps.startColor = normalColor;
            light.color = normalColor;
        }
    }
}
