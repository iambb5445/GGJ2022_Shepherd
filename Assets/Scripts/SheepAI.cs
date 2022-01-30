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
    [SerializeField]
    Material damagedMaterial;
    [SerializeField]
    Renderer[] damagables;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioSource hit;

    Vector3 destination;
    float stopTimer;
    float soundTimer;
    State state;
    NavMeshPath path;
    int navmeshIndex;
    Vector3 tempDestination;
    public static List<GameObject> sheepList = new List<GameObject>();
    float lastDamage = 0f;
    float health = 100f;
    public bool isDead = false;
    Material mainMaterial;
    public static NavMeshPath getNearestSheepPath(Vector3 position)
    {
        float bestDistance = 100000;
        NavMeshPath bestPath = new NavMeshPath();
        for (int i = 0; i < sheepList.Count; i++)
        {
            if (sheepList[i].GetComponent<SheepAI>().isDead)
            {
                continue;
            }
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(position, sheepList[i].transform.position, NavMesh.AllAreas, path);
            float distance = 0;
            for (int j = 0; j < path.corners.Length - 1; j++)
            {
                distance += (path.corners[j] - path.corners[j + 1]).magnitude;
            }
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestPath = path;
            }
        }
        return bestPath;
    }
    public static void checkForDamage(Vector3 position)
    {
        for (int i = 0;i < sheepList.Count; i++)
        {
            float distance = (position - sheepList[i].transform.position).magnitude;
            if (distance < 1f)
            {
                sheepList[i].GetComponent<SheepAI>().damage();
            }
        }
    }

    public void damage()
    {
        if (lastDamage >= 0.5f)
        {
            health -= 20f;
            lastDamage = 0f;
            for (int i = 0; i < damagables.Length; i++)
            {
                damagables[i].material = damagedMaterial;
            }
            hit.Play();
        }
    }
    void Start()
    {
        state = State.standing;
        stopTimer = Utility.getRandomGuassian(5f, 1f);
        soundTimer = Utility.getRandomGuassian(3f, 1f);
        // TODO duality instead
        sheepList.Add(gameObject);
        mainMaterial = damagables[0].material;
        audioSource.pitch = Utility.getRandomGuassian(1f, 0.1f);
    }

    void Update()
    {
        if (soundTimer < 0f)
        {
            soundTimer = Utility.getRandomGuassian(3f, 1f);
            audioSource.Play();
        }
        soundTimer -= Time.deltaTime;
        if (health <= 0f)
        {
            isDead = true;
        }
        if (lastDamage < 0.5f)
        {
            lastDamage += Time.deltaTime;
        }
        if (lastDamage > 0.1f)
        {
            for (int i = 0; i < damagables.Length; i++)
            {
                if (damagables[i].material != mainMaterial)
                {
                    damagables[i].material = mainMaterial;
                }
            }
        }
        if (grabTargetComponent == null || grabTargetComponent.isFree())
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
