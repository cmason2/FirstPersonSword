using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget(player.transform.position);
    }

    private void FaceTarget(Vector3 destination) //https://stackoverflow.com/questions/35861951/unity-navmeshagent-wont-face-the-target-object-when-reaches-stopping-distance
    {
        Vector3 lookPos = destination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f);
    }
}
