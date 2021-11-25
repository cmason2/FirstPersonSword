using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger1 : MonoBehaviour
{
    [SerializeField]
    GameObject bridge1;
    
 
    private void OnTriggerEnter(Collider other)
    {

        movebridge(2f);
           

    }
    private void OnTriggerExit(Collider other)
    {
        bridge1.transform.position += new Vector3(-3, 0, 0);
        
    }
    
    public IEnumerator movebridge(float time)
    {
        Vector3 destination = bridge1.transform.position + new Vector3(3, 0, 0);
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
          bridge1.transform.position= Vector3.Lerp(bridge1.transform.position,destination, t);
            yield return null;
        }
    }
}
