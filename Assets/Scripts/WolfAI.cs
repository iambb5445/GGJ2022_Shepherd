using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;

    float timer = -1;
    NavMeshPath path;
    int pathIndex = 0;
    float speed = 1.5f;
    void Start()
    {
    }

    void Update()
    {
        SheepAI.checkForDamage(transform.position);
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 0.5f;
            path = SheepAI.getNearestSheepPath(transform.position);
            pathIndex = 0;
        }
        if (pathIndex < path.corners.Length)
        {
            Vector3 target = path.corners[pathIndex];
            target.y = transform.position.y;
            Vector3 direction = target - transform.position;
            if (direction.magnitude < 0.1f || (pathIndex == (path.corners.Length - 1)) && direction.magnitude < 0.7f)
            {
                pathIndex += 1;
            }
            else
            {
                transform.LookAt(target);
                Vector3 movement = direction.normalized * Time.deltaTime * speed;
                characterController.Move(movement);
            }
            
        }

    }
}
