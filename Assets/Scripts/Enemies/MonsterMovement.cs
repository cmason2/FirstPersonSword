using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    public float chaseDistance = 10f;
    [SerializeField] float closeRotationSpeed = 0.05f;
    [SerializeField] float attackDistance = 3f;
    [SerializeField] float attackDelay = 2f;
    Vector3 initialPosition;
    Vector3 oldRotation;
    float timeSinceAttack = 0f;
    GameObject target;
    Player playerScript;
    NavMeshAgent navAgent;
    NavMeshPath navPath;
    Animator anim;
    LayerMask playerLayer;
    IEnumerator returnToInitial;
    bool returnToInitialRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navPath = new NavMeshPath();
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
        if(DistanceToPlayer() <= navAgent.stoppingDistance)
        {
            FaceTarget(target.transform.position);
        }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z; //Check if moving forward
        anim.SetFloat("MoveSpeed", speed);
    }

    private void SetDestination()
    {
        returnToInitial = WaitBeforeReturn();
        //https://answers.unity.com/questions/1254520/how-to-check-if-agent-destination-can-be-reached.html
        if (navAgent.CalculatePath(target.transform.position, navPath) 
            && navPath.status == NavMeshPathStatus.PathComplete && DistanceToPlayer() < chaseDistance)
        {
            StopCoroutine(returnToInitial);    
            navAgent.SetPath(navPath);
        }
        else if (DistanceToPlayer() < chaseDistance/2)
        {
            if(!returnToInitialRunning)
                StartCoroutine(returnToInitial);
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
            anim.SetTrigger("Attack");
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
        //First hit check
        yield return new WaitForSeconds(0.25f);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.green, 100f, false);
        if (Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), transform.forward, out RaycastHit hitInfo, attackDistance, playerLayer))
        {
            playerScript.TakeDamage(10);
            Debug.Log("Player taken damage from " + name);
        }
        
        //Second hit check
        yield return new WaitForSeconds(0.68f);
        //Debug.DrawRay(transform.position + new Vector3(0, 1, 0), transform.forward, Color.green, 100f, false);
        if (Physics.Raycast(transform.position + new Vector3(0, 1.5f, 0), transform.forward, out RaycastHit hitInfo2, attackDistance, playerLayer))
        {
            playerScript.TakeDamage(15);
            Debug.Log("Player taken damage from " + name);
        }
    }

    private IEnumerator WaitBeforeReturn()
    {
        returnToInitialRunning = true;
        float timer = 0.5f;
        while(timer > 0)
        {
            FaceTarget(target.transform.position);
            timer -= Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("Roar");
        yield return new WaitForSeconds(1.5f);
        navAgent.destination = initialPosition;
        returnToInitialRunning = false;
    }
}
