using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    Bridge bridge;
    public bool isActivated = false;

    void Start()
    {
        bridge = GameObject.Find("Bridge").GetComponent<Bridge>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Dead Spider" || other.gameObject.tag == "Player")
        {
            isActivated = true;
            bridge.CheckPadStatus();
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Dead Spider" || other.gameObject.tag == "Player")
        {
            isActivated = false;
            bridge.CheckPadStatus();
        }

    }
}
