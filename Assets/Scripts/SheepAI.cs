using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAI : MonoBehaviour
{
    enum State
    {
        moving,
        rotating,
        standing
    }

    [SerializeField]
    float speed = 5f;
    [SerializeField]
    float rotationSpeed = 50f;
    [SerializeField]
    GrabTarget grabTargetComponent;
    [SerializeField]
    CharacterController characterController;

    Vector3 destination;
    float stopTimer;
    State state;
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
                    state = State.rotating;
                    float x = Utility.getRandomGuassian(0f, 1f);
                    float z = Utility.getRandomGuassian(0f, 1f);
                    destination = new Vector3(x, transform.position.y, z);
                }
            }
            else if (state == State.rotating)
            {
                float maxDelta = rotationSpeed * Time.deltaTime;
                Quaternion targetRotation = Quaternion.LookRotation(destination - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDelta);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    state = State.moving;
                }
            }
            else if (state == State.moving)
            {
                destination.y = transform.position.y;
                Vector3 movement = (destination - transform.position).normalized * Time.deltaTime * speed;
                characterController.Move(movement);
                float distance = (transform.position - destination).magnitude;
                if (distance < 0.1f)
                {
                    stopTimer = Utility.getRandomGuassian(5f, 1f);
                    state = State.standing;
                }
            }
            Debug.Log(state);
        }
        else
        {
            state = State.standing;
        }
    }
}
