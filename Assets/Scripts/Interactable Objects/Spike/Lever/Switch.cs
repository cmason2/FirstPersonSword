using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    GameObject[] exitSpikes;
    [SerializeField] private Animator spike;

    private void Start()
    {
        exitSpikes = GameObject.FindGameObjectsWithTag("exit_Door_spike");
    }
    public void MoveSpikesDown()
    {
        foreach(GameObject spike in exitSpikes)
        {
            spike.GetComponent<Animator>().SetTrigger("godown");
        }
    }
}

