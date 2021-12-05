using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatMovement : MonoBehaviour
{
    [SerializeField] float chaseDistance = 10f;
    [SerializeField] float closeRotationSpeed = 0.05f;
    [SerializeField] float attackDistance = 3f;
    [SerializeField] float attackDelay = 3f;
    Vector3 initialPosition;
    Vector3 oldRotation;
    float timeSinceAttack = 0f;
    GameObject target;
    Player playerScript;
    NavMeshAgent navAgent;
    LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("MainCamera");
        playerScript = target.GetComponentInChildren<Player>();
        playerLayer = LayerMask.GetMask("Player");
        initialPosition = transform.position;
        oldRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        SetDestination();
        Attack();
    }

    private void SetDestination()
    {
        if (navAgent.enabled)
        {
            float distance = DistanceToPlayer();
            if (distance < chaseDistance)
            {
                navAgent.destination = target.transform.position; //Stay at current location
                if (distance <= navAgent.stoppingDistance) //Check if agent is within stopping distance
                {
                    FaceTarget(target.transform.position);
                }

                if (!navAgent.pathPending && !navAgent.hasPath) //Check if agent has nowhere to go
                {
                    FaceTarget(target.transform.position);
                }
            }
            else
            {
                navAgent.destination = initialPosition;
            } 
        }
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    private void Attack()
    {
        timeSinceAttack += Time.deltaTime;
        if (DistanceToPlayer() < attackDistance && timeSinceAttack > attackDelay)
        {
            Debug.Log(name + " is attacking");
            timeSinceAttack = 0f;
            navAgent.enabled = false;
            StartCoroutine(AttackCheck());  
        }
    }

    private void FaceTarget(Vector3 destination) //https://stackoverflow.com/questions/35861951/unity-navmeshagent-wont-face-the-target-object-when-reaches-stopping-distance
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, closeRotationSpeed);
    }

    private IEnumerator AttackCheck()
    {
        Vector3 attackStartPosition = transform.position;
        Vector3 targetPosition = target.transform.position;
        
        //Dive towards player
        for (var t = 0f; t < 1; t += Time.deltaTime / 0.6f)
        {
            transform.position = Vector3.Lerp(attackStartPosition, targetPosition, t);
            yield return null;
        }

        //Check if bat got close enough to player to do damage
        if ((transform.position - target.transform.position).magnitude < 1f)
        {
            if (!playerScript.isInvulnerable)
            {
                playerScript.TakeDamage(10);
                Debug.Log("Player taken damage from " + name);
            }
        }

        //Return to position before attack
        for (var t = 0f; t < 1; t += Time.deltaTime / 1f)
        {
            transform.position = Vector3.Lerp(targetPosition, attackStartPosition, t);
            yield return null;
        }
        navAgent.enabled = true;
        //Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward, Color.green, 100f, false);
    }
}
