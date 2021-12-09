using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_door : MonoBehaviour
{
    Animator[] animators;

    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();

    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (Animator animators in animators)
        {

            animators.SetTrigger("open");
        }


    }
}
