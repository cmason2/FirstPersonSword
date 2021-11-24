using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 8.0f;
    public float minDist = 2.0f;

    private float dist;
    private Animator animator;





    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            animator.SetBool("isStop", true);
            return;
        }
        dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > minDist)
        {
            transform.LookAt(target.transform);
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

       
    }
}
