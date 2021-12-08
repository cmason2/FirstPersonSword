using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int hp;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
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

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
