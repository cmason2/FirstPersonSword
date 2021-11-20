using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float homingAmount = 10f;
    public bool isFired = false;
    public bool isHoming = false;
    private bool isMoving = false;
    public Vector3 direction;
    CharacterController player;
    Rigidbody rb;

    private void Start()
    {
        player = FindObjectOfType<CharacterController>();
        rb = GetComponent<Rigidbody>();
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
            Debug.Log("Player hit!");
            obj.GetComponentInChildren<MouseLook>().HP -= 10;
        }
        else if (obj.gameObject.tag == "Environment")
        {
            Destroy(gameObject);
        }
    }
}
