using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTarget : MonoBehaviour
{
    [SerializeField]
    GameObject highlight;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    float groundCheckDistance = 0.4f;
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    float gravityCoefficient = 1f;

    Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;

    Transform holdPoint = null;

    public void underTarget()
    {
        highlight.SetActive(true);
    }

    public void notUnderTarget()
    {
        highlight.SetActive(false);
    }

    public void grab(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
    }

    public void release()
    {
        this.holdPoint = null;
    }

    void Start()
    {
    }

    void Update()
    {
        if (holdPoint != null)
        {
            transform.position = holdPoint.position;
        }
        else
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayerMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime * gravityCoefficient;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
