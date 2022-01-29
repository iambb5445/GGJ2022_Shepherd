using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    float speed = 12f;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    float groundCheckDistance = 0.4f;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    float jumpHeight = 3f;

    Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;
    void Start()
    {
        
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        characterController.Move(move * speed * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, layerMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }
}
