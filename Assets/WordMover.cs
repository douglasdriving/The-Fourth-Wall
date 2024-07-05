using System.Collections.Generic;
using UnityEngine;

public class WordMover : MonoBehaviour
{
    [SerializeField] float heightAbovePlatform = 0.8f;
    [SerializeField] float minDistanceForPosChange = 5f;
    [SerializeField] float minimumDistanceFromPlayer = 10f;
    GameObject currentPlatform = null;

    void Awake()
    {
        UpdateWordPosition();
    }

    void Update()
    {
        float distanceToPlayer = DistanceFromGameObjectToPlayer(gameObject);
        bool playerIsTooCloseToWord = distanceToPlayer < minDistanceForPosChange;
        if (playerIsTooCloseToWord)
        {
            UpdateWordPosition();
        }
    }

    private void UpdateWordPosition()
    {
        List<GameObject> platformsSeenByCamera = GetPlatformsInView();
        GameObject closestPlatform = GetClosestPlatform(platformsSeenByCamera);
        PositionAbovePlatform(closestPlatform);
        RotateTowardsPlayer();
        currentPlatform = closestPlatform;
    }

    private List<GameObject> GetPlatformsInView()
    {
        Camera cam = Camera.main;
        GameObject[] platformsInScene = GameObject.FindGameObjectsWithTag("Platform");
        List<Renderer> platformRenderers = new();
        foreach (GameObject platform in platformsInScene)
        {
            platformRenderers.Add(platform.GetComponent<Renderer>());
        }
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        List<GameObject> platformsInView = new();
        foreach (Renderer platformRenderer in platformRenderers)
        {
            if (GeometryUtility.TestPlanesAABB(frustumPlanes, platformRenderer.bounds))
            {
                platformsInView.Add(platformRenderer.gameObject);
            }
        }
        return platformsInView;
    }

    private GameObject GetClosestPlatform(List<GameObject> platforms)
    {
        GameObject platformAtGoodDistance = null;
        foreach (GameObject platform in platforms)
        {
            //make sure its far enough from the player
            float distanceToPlayer = DistanceFromGameObjectToPlayer(platform);
            bool isFarEnoughFromPlayer = distanceToPlayer > minimumDistanceFromPlayer;
            if (!isFarEnoughFromPlayer)
            {
                continue;
            }

            //if its the first platform we see, save it
            if (platformAtGoodDistance == null)
            {
                platformAtGoodDistance = platform;
                continue;
            }

            //check if its the closest platform to the player
            float distanceToPlayerOfSavedPlatform = DistanceFromGameObjectToPlayer(platformAtGoodDistance);
            bool isCloserThanSavedPlatform = distanceToPlayer < distanceToPlayerOfSavedPlatform;
            if (isFarEnoughFromPlayer && isCloserThanSavedPlatform)
            {
                platformAtGoodDistance = platform;
            }
        }
        return platformAtGoodDistance;
    }

    private float DistanceFromGameObjectToPlayer(GameObject platform)
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 platformPos = platform.transform.position;
        float distance = Vector3.Distance(playerPos, platformPos);
        return distance;
    }

    private void PositionAbovePlatform(GameObject closestPlatform)
    {
        transform.position = closestPlatform.transform.position;
        Vector3 platformPos = closestPlatform.transform.position;
        float platformHeight = closestPlatform.transform.lossyScale.y;

        float wordPosX = platformPos.x;
        float wordPosY = platformPos.y + platformHeight * heightAbovePlatform;
        float wordPosZ = platformPos.z;

        transform.position = new Vector3(wordPosX, wordPosY, wordPosZ);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 directionVector = (transform.position - playerPos).normalized;
        transform.forward = directionVector;
    }
}
