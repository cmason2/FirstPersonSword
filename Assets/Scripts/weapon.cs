using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    public int weaponDamage = 10;

    float tempTime = 0;
    public float cd = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" )//&& Input.GetButton("Fire1"))
        {
          if(Time.time - tempTime > cd)
            {
                other.gameObject.GetComponent<EnemyStatic>().Takedamage(weaponDamage);

                tempTime = Time.time;

            }
            
        }
    }
}
