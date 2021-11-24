using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : MonoBehaviour
{
    public int enemyHealth = 100;
    public int currentHealth;
    public int enemyDamage = 10;



    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemyHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Takedamage(int _damage)
    {
        currentHealth -= _damage;
    }

    
}
