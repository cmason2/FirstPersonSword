using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderMovement : MonoBehaviour
{
    public float chaseDistance = 10f;
    [SerializeField] float closeRotationSpeed = 0.05f;
    [SerializeField] float attackDistance = 2f;
    [SerializeField] float attackDelay = 2f;
    Vector3 initialPosition;
    Vector3 oldRotation;
    float timeSinceAttack = 0f;
    GameObject target;
    Player playerScript;
    NavMeshAgent navAgent;
    Animator anim;
    LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player");
        playerScript = target.GetComponentInChildren<Player>();
        playerLayer = LayerMask.GetMask("Player");
        initialPosition = transform.position;
        oldRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        SetDestination();
        UpdateAnimator();
        Attack();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z; //Check if moving forward
        anim.SetFloat("forwardSpeed", speed);
    }

    private void SetDestination()
    {
        float distance = DistanceToPlayer();
        if (distance < chaseDistance)
        {
            anim.SetBool("inRangeOfPlayer", true);
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
            anim.SetBool("inRangeOfPlayer", false);
            navAgent.destination = initialPosition;
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
            //if(Random.value > 0.5)
            //{
            //    anim.SetTrigger("triggerAttack1");
            //}
            //else
            //{
            //    anim.SetTrigger("triggerAttack2");
            //}
            anim.SetTrigger("triggerAttack1");
            Debug.Log(name + " is attacking");
            timeSinceAttack = 0f;
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
        yield return new WaitForSeconds(0.5f);
        //Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward, Color.green, 100f, false);
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out RaycastHit hitInfo, attackDistance, playerLayer))
        {
            if (!playerScript.isInvulnerable)
            {
                playerScript.TakeDamage(10);
                Debug.Log("Player taken damage from " + name);
            }
        }
    }
}
