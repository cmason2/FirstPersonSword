using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{

    CharacterController player;

    public GameObject energyBall;

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
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, startPosition.y + 1.5f, endPosition.z);
        StartCoroutine(VerticalOscillation());
        StartCoroutine(ShootSingle(true));
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
        while (true)
        {
            //Instantiate, parent and position energy ball
            GameObject ball = Instantiate(energyBall, transform);
            ball.transform.localScale = new Vector3(0f, 0f, 0f);
            ball.transform.localPosition = new Vector3(0f, 0.09f, 1.8f);

            //Grow energy ball
            while (ball.transform.localScale.x < 1f - 0.1)
            {
                ball.transform.localScale = Vector3.SmoothDamp(ball.transform.localScale, new Vector3(1.1f, 1.1f, 1.1f), ref scaleVelocity, 1f);
                yield return null;
            }

            ball.transform.parent = null;
            if(homing)
            {
                ball.GetComponent<Projectile>().isHoming = true;
            }
            
            ball.GetComponent<Projectile>().isFired = true;

            yield return new WaitForSeconds(reloadTime);
        }
    }
}
