using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_switch : MonoBehaviour
{
    Animator[] animators;
    GameObject informationTextObject;
    [SerializeField] float textDisplayTime = 2f;
    [SerializeField] float textFadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        informationTextObject = GameObject.Find("InformationText");

    }

    private void OnTriggerEnter(Collider collider)
    {
        foreach (Animator animators in animators)
        {

            animators.SetTrigger("blockexit");
 
        }
        StartCoroutine(informationTextObject.GetComponent<TextFadeInOut>().DisplayTextFade("Find the switch to disable the spikes", textDisplayTime, textFadeTime));
    }
}
