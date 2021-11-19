using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 5f;

    private void Start()
    {
        
    }

    void Update()
    {
        float rot = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //float rot = Input.GetAxis("lookaround");

        //Vector3 moveCal = transform.right * x + transform.forward * z;
        Vector3 moveCal = transform.forward * z;

        controller.Move(moveCal * playerSpeed * Time.deltaTime);

        GetComponent<Transform>().Rotate(Vector3.up * rot * 3);
    }

}
