using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    [SerializeField]
    CharacterController characterController;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask groundLayerMask;
    [SerializeField]
    float groundCheckDistance = 0.4f;
    [SerializeField]
    float gravityCoefficient = 1f;

    float timer = -1;
    NavMeshPath path;
    int pathIndex = 0;
    float speed = 1.5f;
    Vector3 velocity;
    float gravity = -9.81f;
    bool isGrounded;
    public static List<GameObject> nightWolfList = new List<GameObject>();
    public static bool wolfsCanReach(Vector3 position)
    {
        Vector3 groundPosition = new Vector3(position.x, 0.5f, position.y);
        for (int i = 0; i < nightWolfList.Count; i++)
        {
            NavMeshPath path = new NavMeshPath();
            Vector3 wolfGroundPosition = nightWolfList[i].transform.position;
            wolfGroundPosition.y = 0.5f;
            
            NavMesh.CalculatePath(wolfGroundPosition, groundPosition, NavMesh.AllAreas, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }
    void Start()
    {
    }

    void Update()
    {
        SheepAI.checkForDamage(transform.position);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayerMask);
        if (isGrounded)
        {
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
                    if (GameManager.isNight)
                    {
                        speed *= 5;
                    }
                    characterController.Move(movement);
                }

            }
        }
        if (isGrounded)
        {
            velocity = Vector3.zero;
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime * gravityCoefficient;
        characterController.Move(velocity * Time.deltaTime);
    }
}
