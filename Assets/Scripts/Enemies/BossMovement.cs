using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{

    CharacterController player;

    public GameObject energyBall;
    public GameObject spiderPrefab;

    [SerializeField] float rotationSpeed = 0.01f;
    [SerializeField] float stopError = 0.1f;
    [SerializeField] float smoothTime = 1f;
    [SerializeField] float reloadTime = 1f;

    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 verticalVelocity = Vector3.zero;
    Vector3 scaleVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterController>();
       
        StartCoroutine(Phase1());
    }

    // Update is called once per frame
    void Update()
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
        endPosition = new Vector3(startPosition.x, startPosition.y + 1.5f, endPosition.z);

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

    IEnumerator ShootSingle(bool homing)
    {
        StartCoroutine(VerticalOscillation());
        while (true)
        {
            Debug.Log("Started shooting");
            //Instantiate, parent and position energy ball
            GameObject ball = Instantiate(energyBall, transform);
            ball.transform.localPosition = new Vector3(0f, 0.09f, 1.8f);

            yield return new WaitForSeconds(2f);

            ball.transform.parent = null;
            if(homing)
            {
                ball.GetComponent<Projectile>().isHoming = true;
            }
            
            ball.GetComponent<Projectile>().isFired = true;

            yield return new WaitForSeconds(reloadTime);
        }
    }

    IEnumerator SpawnEnemies()
    {
        Debug.Log("Started enemy spawning");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + new Vector3(transform.position.x, -4.5f, transform.position.z);
        //Move boss down to ground level
        for (var t = 0f; t < 1; t += Time.deltaTime / 3f)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        //Spawn spider enemies
        for (int i = 0; i < 2; i++)
        {
            GameObject spider = Instantiate(spiderPrefab, transform);
            spider.transform.localPosition = new Vector3(0f, 0f, 1.8f);
            spider.transform.localScale = Vector3.zero;
            NavMeshAgent spiderAgent = spider.GetComponent<NavMeshAgent>();
            SpiderMovement spiderMovement = spider.GetComponent<SpiderMovement>();
            spiderMovement.enabled = false;
            spiderAgent.enabled = false;
            for (var t = 0f; t < 1; t += Time.deltaTime / 2f)
            {
                spider.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

            spider.transform.parent = null;
            Rigidbody rb = spider.AddComponent<Rigidbody>();
            rb.AddForce(transform.forward * 30f);
            yield return new WaitForSeconds(1f);
            Destroy(rb);
            spiderAgent.enabled = true;
            spiderMovement.enabled = true;
            yield return new WaitForSeconds(2f);
        }

        //Move boss upwards to normal height
        for (var t = 0f; t < 1; t += Time.deltaTime / 3f)
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, t);
            yield return null;
        }
        Debug.Log("Finished spawning enemies");
    }

    IEnumerator Phase1()
    {
        StartCoroutine(ShootSingle(true));
        yield return new WaitForSeconds(5f);
        StopAllCoroutines();
        Debug.Log("Started spawning enemies");
        yield return StartCoroutine(SpawnEnemies());
        Debug.Log("Stopped spawning enemies");
        StartCoroutine(ShootSingle(true));
    }
}
