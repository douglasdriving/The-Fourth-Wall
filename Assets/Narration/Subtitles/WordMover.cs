using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WordMover : MonoBehaviour
{
    [SerializeField] LayerMask layerToCheckForVisionBlock;
    [SerializeField] float distanceForChange = 5f;
    [SerializeField] float minDistanceForPlatformToJumpTo = 10f;
    [SerializeField] TMP_Text word;
    [SerializeField] bool moveWordWhenPlayerIsTooClose = false;

    Camera cam;
    Transform playerTransform;

    [SerializeField] Transform wordPointsParent;
    List<Vector3> wordPositionsInScene = new();

    void Awake()
    {
        SetupSceneReferences();
        UpdateWordPosition();
    }

    private void SetupSceneReferences()
    {
        cam = Camera.main;
        foreach (Transform wordPoint in wordPointsParent)
        {
            wordPositionsInScene.Add(wordPoint.position);
        }
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (moveWordWhenPlayerIsTooClose)
        {
            float distanceFromWordToPlayer = Vector3.Distance(playerTransform.position, transform.position);
            bool playerIsTooCloseToWord = distanceFromWordToPlayer < distanceForChange;
            if (playerIsTooCloseToWord)
            {
                UpdateWordPosition();
            }
        }
    }

    public void UpdateWordPosition()
    {
        Vector3[] visiblePoints = GetVisiblePoints();
        Vector3? bestPoint = GetPointAtBestDistance(visiblePoints);
        if (bestPoint == null)
        {
            word.enabled = false;
        }
        else
        {
            word.enabled = true;
            transform.position = (Vector3)bestPoint;
            RotateTowardsCamera();
        }
    }

    private Vector3[] GetVisiblePoints()
    {
        Vector3[] visiblePoints = wordPositionsInScene.Where(point => IsPointVisible(point)).ToArray();
        return visiblePoints;
    }

    bool IsPointVisible(Vector3 point)
    {
        // Convert point from world space to viewport space
        Vector3 viewportPoint = cam.WorldToViewportPoint(point);

        // Check if the point is within the viewport bounds and in front of the camera
        bool isInView = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                        viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                        viewportPoint.z > 0;

        if (!isInView) return false;

        // Perform a raycast from the camera to the point
        Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(point));
        bool rayHitSomething = Physics.Raycast(ray, layerToCheckForVisionBlock);
        bool pointIsObstructed = rayHitSomething;
        bool pointIsVisible = !pointIsObstructed;

        return pointIsVisible;
    }

    private Vector3? GetPointAtBestDistance(Vector3[] points)
    {
        Vector3? bestPoint = null;
        foreach (Vector3 point in points)
        {
            //make sure its far enough from the player
            float distanceToPlayer = Vector3.Distance(playerTransform.position, point);
            bool isFarEnoughFromPlayer = distanceToPlayer > minDistanceForPlatformToJumpTo;
            if (!isFarEnoughFromPlayer)
            {
                continue;
            }

            //if its the first platform we see, save it
            if (bestPoint == null)
            {
                bestPoint = point;
                continue;
            }

            //check if its the closest platform to the player
            float distanceBetweenPlayerAndCurrentBestPoint = Vector3.Distance(playerTransform.position, (Vector3)bestPoint);
            bool isCloserThanCurrentBestPoint = distanceToPlayer < distanceBetweenPlayerAndCurrentBestPoint;
            if (isCloserThanCurrentBestPoint)
            {
                bestPoint = point;
            }
        }

        return bestPoint;
    }

    private void RotateTowardsCamera()
    {
        Vector3 directionVector = (transform.position - cam.transform.position).normalized;
        transform.forward = directionVector;
    }
}
