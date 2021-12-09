using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switch2 : MonoBehaviour
{
    [SerializeField] private Animator spike;



    private void OnTriggerEnter(Collider other)
    {


        spike.SetTrigger("godown");
    }

}
