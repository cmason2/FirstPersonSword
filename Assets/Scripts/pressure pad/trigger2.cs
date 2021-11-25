using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger2 : MonoBehaviour
{
    [SerializeField]
    GameObject bridge2;
    
    private void OnTriggerEnter(Collider other)
    {
      
            bridge2.transform.position += new Vector3(-3, 0, 0);
    }
    private void OnTriggerExit(Collider other)
    {
        bridge2.transform.position += new Vector3(3, 0, 0);
    }
}
