using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 3f;
    [SerializeField] float homingAmount = 10f;
    public bool isFired = false;
    public bool isHoming = false;
    private bool isMoving = false;
    private float timeActive = 0f;
    public Vector3 direction;
    CharacterController player;
    Player playerScript;
    Rigidbody rb;

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        rb = GetComponent<Rigidbody>();
        playerScript = player.GetComponentInChildren<Player>();
    }

    private void Update()
    {
        timeActive += Time.deltaTime;
        if(isFired)
        {
            direction = player.GetComponentInChildren<Camera>().transform.position - rb.position;
            direction.Normalize();
            isFired = false;
            isMoving = true;
        }
        if (timeActive > 10)
            GetComponent<ParticleSystem>().Stop();
    }

    private void FixedUpdate()
    {
        if(isMoving)
        {
            if(isHoming)
            {
                direction = player.GetComponentInChildren<Camera>().transform.position - rb.position;
                direction.Normalize();
                Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
                rb.angularVelocity = rotationAmount * homingAmount;
                rb.velocity = transform.forward * projectileSpeed;
            }
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
        else if (obj.gameObject.tag == "Environment")
        {
            rb.isKinematic = true;
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
