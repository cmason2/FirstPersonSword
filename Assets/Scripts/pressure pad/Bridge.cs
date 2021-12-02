using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    Pad[] pressurePads;
    Vector3 startposition;
    Vector3 endposition;

    void Start()
    {
        pressurePads = GameObject.FindObjectsOfType<Pad>();
        startposition = transform.position;
        endposition = startposition + new Vector3(13f, 0, 0);
    }
    public IEnumerator bridgemoveforward(float time)
    {

        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(transform.position, endposition, t);
            yield return null;
        }
    }
    public IEnumerator bridgemovebackward(float time)
    {

        for (var t = 0f; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(transform.position, startposition, t);
            yield return null;
        }
    }

    public void CheckPadStatus()
    {
        bool allPadsActivated = true;
        foreach (Pad pad in pressurePads)
        {
            if (!pad.isActivated)
                allPadsActivated = false;
        }

        StopAllCoroutines();
        if (allPadsActivated)
        {
            StartCoroutine(bridgemoveforward(50f));
        }
        else
        {
            StartCoroutine(bridgemovebackward(50f));
        }
    }
}
