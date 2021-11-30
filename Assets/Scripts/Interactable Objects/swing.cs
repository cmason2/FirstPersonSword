using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swing : MonoBehaviour
{
    Animator s_animator;
    // Start is called before the first frame update
    void Start()
    {
        s_animator = GetComponent<Animator>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            s_animator.SetTrigger("click");
        }
    }
}
