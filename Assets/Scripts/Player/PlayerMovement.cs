using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float playerSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    
    bool isGrounded;
    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        //Update x,z positions (ground plane)
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = -0.5f;
        }

        velocity.y += gravity * Time.deltaTime;

        //Update y position (vertical)
        controller.Move(velocity * Time.deltaTime);
    }
}
