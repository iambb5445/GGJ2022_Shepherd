using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepAI : MonoBehaviour
{
    enum State
    {
        moving,
        rotating,
        standing,
        cornerCalculate
    }

    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float rotationSpeed = 50f;
    [SerializeField]
    GrabTarget grabTargetComponent;
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    LayerMask groundLayerMask;

    Vector3 destination;
    float stopTimer;
    State state;
    NavMeshPath path;
    int navmeshIndex;
    Vector3 tempDestination;
    void Start()
    {
        state = State.standing;
        stopTimer = Utility.getRandomGuassian(5f, 1f);
    }

    void Update()
    {
        if (grabTargetComponent.isFree())
        {
            if (state == State.standing)
            {
                stopTimer -= Time.deltaTime;
                if (stopTimer < 0)
                {
                    path = new NavMeshPath();
                    float x = Utility.getRandomGuassian(0f, 1f);
                    float z = Utility.getRandomGuassian(0f, 1f);
                    destination = new Vector3(transform.position.x + x, 45f, transform.position.z + z);
                    Physics.Raycast(destination, Vector3.down, out RaycastHit hitInfo, 50f, groundLayerMask);
                    destination = hitInfo.point;

                    NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
                    state = State.cornerCalculate;
                    navmeshIndex = 1;
                }
            }
            else if (state == State.cornerCalculate)
            {
                if (navmeshIndex >= path.corners.Length)
                {
                    stopTimer = Utility.getRandomGuassian(5f, 1f);
                    state = State.standing;
                }
                else
                {
                    tempDestination = path.corners[navmeshIndex];
                    state = State.rotating;
                }
            }
            else if (state == State.rotating)
            {
                float maxDelta = rotationSpeed * Time.deltaTime;
                Vector3 lookTarget = new Vector3(tempDestination.x, transform.position.y, tempDestination.z);
                Quaternion targetRotation = Quaternion.LookRotation(lookTarget - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    state = State.moving;
                }
            }
            else if (state == State.moving)
            {
                tempDestination.y = transform.position.y;
                Vector3 movement = (tempDestination - transform.position).normalized * Time.deltaTime * speed;
                characterController.Move(movement);
                float distance = (transform.position - tempDestination).magnitude;
                if (distance < 0.1f)
                {
                    navmeshIndex += 1;
                    state = State.cornerCalculate;
                }
            }
        }
        else
        {
            state = State.standing;
        }
    }
}
