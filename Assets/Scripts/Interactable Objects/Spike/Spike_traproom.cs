using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_traproom : MonoBehaviour
{
    Animator animators;
    
    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentInChildren<Animator>();
       
    }

    private void OnTriggerEnter(Collider collider)
    {
     
            animators.SetTrigger("activate");
      


    }

    
}
