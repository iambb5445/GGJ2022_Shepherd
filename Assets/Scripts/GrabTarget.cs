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
    [SerializeField]
    float throwSpeed = 10f;

    Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;
    Vector3 possibleThrow;

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
        velocity = possibleThrow * throwSpeed;
    }

    void Start()
    {
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayerMask);
        if (holdPoint != null)
        {
            Vector3 holdPosition = holdPoint.position;
            if (isGrounded)
            {
                holdPosition.y = Mathf.Max(transform.position.y, holdPosition.y);
            }
            possibleThrow = holdPosition - transform.position;
            transform.position = holdPosition;
        }
        else
        {
            if (isGrounded)
            {
                velocity = Vector3.zero;
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime * gravityCoefficient;
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
