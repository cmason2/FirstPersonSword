using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectProjectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 5f;
    public bool isFired = false;
    private bool isMoving = false;
    public int deflectNumber = 0;
    public int requiredDeflections = 5;
    public Vector3 direction;
    CharacterController player;
    Player playerScript;
    GameObject boss;
    BossEnemy bossEnemyScript;
    BossMovement bossMovementScript;
    Rigidbody rb;

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        playerScript = player.GetComponentInChildren<Player>();
        boss = GameObject.Find("BossEnemy");
        bossEnemyScript = boss.GetComponent<BossEnemy>();
        bossMovementScript = boss.GetComponent<BossMovement>();
        bossMovementScript.deflectBallActive = true;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        bossMovementScript.deflectBallActive = false;
    }

    private void Update()
    {
        if(isFired)
        {
            direction = player.GetComponentInChildren<Camera>().transform.position - rb.position;
            direction.Normalize();
            isFired = false;
            isMoving = true;
        }
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            rb.velocity = direction * projectileSpeed;
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if(!playerScript.isInvulnerable)
            {
                playerScript.TakeDamage(10);
                Debug.Log("Player taken damage from " + name);
            }
        }
        else if (obj.gameObject.name == "BossEnemy")
        {
            if (deflectNumber == requiredDeflections)
            {
                //damage boss, delete projectile
                bossEnemyScript.TakeDamage(20);
                rb.isKinematic = true;
                Destroy(gameObject);
            }
            else if(deflectNumber % 2 == 1)
            {
                //increment deflect
                GetComponent<DeflectEnemy>().TakeDamage(10);
                //change target to player
                direction = player.GetComponentInChildren<Camera>().transform.position - rb.position;
                direction.Normalize();
                projectileSpeed += 2f;
            }
        }
        else if (obj.gameObject.tag == "Environment")
        {
            rb.isKinematic = true;
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
