using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger1 : MonoBehaviour
{
    [SerializeField]
    GameObject bridge1;
    Vector3 startposition;
    Vector3 endposition;

    void Start()
    {
        startposition = bridge1.transform.position;
        endposition = startposition + new Vector3(3, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        StartCoroutine(bridgemoveforward(50f));

    }
    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        StartCoroutine(bridgemovebackward(50f));
      
    }
    
    public IEnumerator bridgemoveforward(float time)
    {
        
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
          bridge1.transform.position= Vector3.Lerp(bridge1.transform.position,endposition, t);
            yield return null;
        }
    }
    public IEnumerator bridgemovebackward(float time)
    {
       
        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            bridge1.transform.position = Vector3.Lerp(bridge1.transform.position,startposition, t);
            yield return null;
        }
    }
}