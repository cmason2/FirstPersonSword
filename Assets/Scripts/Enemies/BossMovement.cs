using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    BossEnemy boss;
    
    CharacterController player;

    public GameObject energyBall;
    public GameObject deflectBall;
    public GameObject spiderPrefab;

    [SerializeField] float rotationSpeed = 0.01f;
    [SerializeField] float stopError = 0.1f;
    [SerializeField] float smoothTime = 1f;
    [SerializeField] float reloadTime = 1f;

    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 verticalVelocity = Vector3.zero;
    Vector3 scaleVelocity = Vector3.zero;

    int wave2HP = 50;
    public bool deflectBallActive = false;

    private SpiderMovement[] spiderEnemies;
    private SpiderEnemy[] allSpiders;
    private Pad[] pressurePads;
    private Bridge bridge;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<BossEnemy>();
        player = FindObjectOfType<CharacterController>();
        pressurePads = GameObject.FindObjectsOfType<Pad>();
        bridge = GameObject.FindObjectOfType<Bridge>();
        StartCoroutine(VerticalOscillation());
        StartCoroutine(Phase1());
        //StartCoroutine(Phase2());
    }

    private void Update()
    {
        FaceTarget(player.transform.position);
    }

    private void FaceTarget(Vector3 destination) //https://stackoverflow.com/questions/35861951/unity-navmeshagent-wont-face-the-target-object-when-reaches-stopping-distance
    {
        Vector3 lookPos = destination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);
    }

    IEnumerator VerticalOscillation()
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, startPosition.y - 0.5f, endPosition.z);
        while (true)
        {
            while (Mathf.Abs(transform.position.y - endPosition.y) > stopError)
            {
                transform.position = Vector3.SmoothDamp(transform.position, endPosition, ref verticalVelocity, smoothTime);
                yield return null;
            }

            while (Mathf.Abs(transform.position.y - startPosition.y) > stopError)
            {
                transform.position = Vector3.SmoothDamp(transform.position, startPosition, ref verticalVelocity, smoothTime);
                yield return null;
            }
        }
    }

    IEnumerator ShootSingle(bool homing, int numShots)
    {
        for (int i = 0; i < numShots; i++)
        {
            if(boss.HP <= wave2HP)
            {
                yield break;
            }
            Debug.Log("Started shooting");
            //Instantiate, parent and position energy ball
            GameObject ball = Instantiate(energyBall, transform);
            ball.transform.localPosition = new Vector3(0f, 0.09f, 1.8f);

            yield return StartCoroutine(HPInterruptedWait(wave2HP, 1f));

            ball.transform.parent = null;
            if(homing)
            {
                ball.GetComponent<Projectile>().isHoming = true;
            }
            
            ball.GetComponent<Projectile>().isFired = true;

            yield return StartCoroutine(HPInterruptedWait(wave2HP, reloadTime));
        }
    }

    IEnumerator ShootDeflect()
    {
        while (true)
        {
            Debug.Log("Started shooting");
            //Instantiate, parent and position energy ball
            GameObject ball = Instantiate(deflectBall, transform);
            deflectBallActive = true;
            ball.transform.localPosition = new Vector3(0f, 0.09f, 1.8f);

            yield return new WaitForSeconds(2f);

            ball.transform.parent = null;

            ball.GetComponent<DeflectProjectile>().isFired = true;

            while (deflectBallActive)
            {
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnEnemies(int numEnemies)
    {
        //Spawn spider enemies, numEnemies = number of enemies spawned during this spawning phase
        for (int i = 0; i < numEnemies; i++)
        {
            GameObject spider = Instantiate(spiderPrefab, transform);
            spider.transform.localPosition = new Vector3(0f, 0f, 1.8f);
            spider.transform.localScale = Vector3.zero;
            NavMeshAgent spiderAgent = spider.GetComponent<NavMeshAgent>();
            SpiderMovement spiderMovement = spider.GetComponent<SpiderMovement>();
            spiderMovement.chaseDistance = 20f;
            spiderMovement.enabled = false;
            spiderAgent.enabled = false;
            
            //Grow Spider
            for (var t = 0f; t < 1; t += Time.deltaTime / 2f)
            {
                if(boss.HP <= wave2HP)
                {
                    Destroy(spider);
                    yield break;
                }
                spider.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

            //Drop Spider
            spider.transform.parent = null;
            Rigidbody rb = spider.AddComponent<Rigidbody>();
            rb.AddForce(transform.forward * 30f);
            yield return StartCoroutine(HPInterruptedWait(wave2HP, 1f)); //Delay before spider becomes active
            Destroy(rb);
            spiderAgent.enabled = true;
            spiderMovement.enabled = true;
            yield return StartCoroutine(HPInterruptedWait(wave2HP, 1f)); //Delay before spawning next spider
        }
    }

    IEnumerator HPInterruptedWait(int healthLimit, float timeToWait)
    {
        for (float time = timeToWait;  timeToWait >= 0; timeToWait -= Time.deltaTime)
        {
            if(boss.HP <= healthLimit)
            {
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator MoveOverTime(Vector3 start, Vector3 end, float time)
    {
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    IEnumerator Phase1()
    {
        while(boss.HP > 50)
        {
            yield return StartCoroutine(ShootSingle(false, 5));
            spiderEnemies = FindObjectsOfType<SpiderMovement>();
            if(spiderEnemies.Length < 2)
            {
                yield return StartCoroutine(SpawnEnemies(2 - spiderEnemies.Length));
            }
        }
        StartCoroutine(Phase2());
    }

    IEnumerator Phase2()
    {
        //Remove all spider enemies, including dead ones (bridge will retract)
        allSpiders = FindObjectsOfType<SpiderEnemy>();
        foreach (SpiderEnemy spider in allSpiders)
        {
            Destroy(spider.gameObject);
        }
        foreach (Pad pad in pressurePads)
        {
            pad.isActivated = false;
        }
        bridge.CheckPadStatus();
        //boss.SetInvulnerability(true);
        StartCoroutine(ShootDeflect());
        yield return null;
    }
}
