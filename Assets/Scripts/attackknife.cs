using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackknife : MonoBehaviour
{
    public Animator knife;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            this.GetComponent<Animator>().SetTrigger("click");
        }
    }
}