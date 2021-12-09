using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_smallspike : MonoBehaviour
{

    CharacterController player;
    Player playerScript;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterController>();
        rb = GetComponent<Rigidbody>();
        playerScript = player.GetComponentInChildren<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            if (!playerScript.isInvulnerable)
            {
                playerScript.TakeDamage(4);
                Debug.Log("Player taken damage from " + name);
            }
        }
        
    }
}
