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
    float throwSpeed = 1f;
    [SerializeField]
    float throwTime = 0.05f;

    Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;
    Vector3 possibleThrow;
    float totalInQueue = 0f;
    Queue<KeyValuePair<Vector3, float>> queue = new Queue<KeyValuePair<Vector3, float>>();

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
        holdPoint = null;
        velocity =  throwSpeed * possibleThrow;
    }

    public bool isFree()
    {
        return isGrounded && holdPoint == null;
    }

    void Start()
    {
    }

    Vector3 clampAboveGround(Vector3 position)
    {
        Vector3 transformToGroundCheck = transform.position - groundCheck.position;
        Vector3 toBeGroundCheck = position - transformToGroundCheck;
        Physics.Raycast(toBeGroundCheck, Vector3.up, out RaycastHit hitInfo, 10f, groundLayerMask);
        if (hitInfo.collider != null)
        {
            return hitInfo.point + transformToGroundCheck;
        }
        return position;
    }

    void Update()
    {
        if (GameManager.nightTrigger)
        {
            notUnderTarget();
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayerMask);
        if (holdPoint != null)
        {
            Vector3 holdPosition = holdPoint.position;
            holdPosition = clampAboveGround(holdPosition);
            transform.position = holdPosition;

            queue.Enqueue(new KeyValuePair<Vector3, float>(transform.position, Time.deltaTime));
            totalInQueue += Time.deltaTime;
            while (totalInQueue > throwTime)
            {
                totalInQueue -= queue.Dequeue().Value;
            }
            if (queue.Count == 0)
            {
                possibleThrow = Vector3.zero;
            }   
            else
            {
                possibleThrow = (transform.position - queue.Peek().Key) / totalInQueue;
            }
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
