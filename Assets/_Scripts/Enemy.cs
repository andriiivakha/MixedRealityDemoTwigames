using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bricks;
    [SerializeField] private LayerMask destructableLayerMask;

    private Brick targetBrick;
    private float enemySpeed = 3f;
    private float timeToStayAtPoint = 1f;
    private bool isAtPointOfInterest = false;
    private float timeAtPoint = 0f;
    private Vector3 startingPosition;
    private bool isBrickTaken = false;
    private MeshRenderer mesh;

    private void Awake()
    {
        bricks.SetActive(false);
        mesh = GetComponentInChildren<MeshRenderer>();
    }

    private void Start()
    {
        startingPosition = transform.position;
        if (Physics.SphereCast(transform.position, 10f, GameController.Instance.PlayerTransform.position - transform.position,
            out RaycastHit hit, float.MaxValue, destructableLayerMask))
        {
            targetBrick = hit.collider.gameObject.GetComponent<Brick>();
        }
    }

    public void Setup(Brick brickToTake)
    {
        targetBrick = brickToTake;
    }

    void Update()
    {
        if (targetBrick == null)
            return;

        if (!isAtPointOfInterest)
        {
            MoveTo(targetBrick.transform.position);

            // Check if the enemy has reached the point of interest
            float distanceToTarget = Vector3.Distance(transform.position, targetBrick.transform.position);
            if (distanceToTarget < 0.5f)  // Adjust the threshold as needed
            {
                isAtPointOfInterest = true;
                timeAtPoint = 0f;
            }
        }
        else
        {
            // Enemy is at the point of interest
            timeAtPoint += Time.deltaTime;
            if (!isBrickTaken)
            {
                isBrickTaken = true;
                targetBrick.Destroy();
                bricks.SetActive(true);
            }

            if (timeAtPoint >= timeToStayAtPoint)
            {
                // Time to move back to the starting position
                MoveTo(startingPosition);

                // Check if the enemy has reached the starting position
                float distanceToStartingPos = Vector3.Distance(transform.position, startingPosition);
                if (distanceToStartingPos < 0.5f)  // Adjust the threshold as needed
                {
                    // Enemy is back to starting position
                    SelfDestroy();
                }
            }
        }
        AddJiggle();
    }

    void MoveTo(Vector3 targetPosition)
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);
        transform.LookAt(targetPosition);
    }

    private void AddJiggle()
    {
        float newY = startingPosition.y + .15f * Mathf.Sin(1f * Time.time);
        mesh.transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void SelfDestroy()
    {
        // Implement logic for self-destruction
        Destroy(gameObject);
    }
}
