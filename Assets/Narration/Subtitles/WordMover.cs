using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordMover : MonoBehaviour
{
    [SerializeField] float heightAbovePlatform = 0.8f;
    [SerializeField] float distanceForChange = 5f;
    [SerializeField] float minDistanceForPlatformToJumpTo = 10f;
    [SerializeField] TMP_Text word;

    Camera cam;
    GameObject[] platformsInScene;
    List<Renderer> platformRenderersInScene = new();
    Transform playerTransform;


    void Awake()
    {
        SetupSceneReferences();
        UpdateWordPosition();
    }

    private void SetupSceneReferences()
    {
        cam = Camera.main;
        platformsInScene = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platformsInScene)
        {
            platformRenderersInScene.Add(platform.GetComponent<Renderer>());
        }
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        float distanceFromWordToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        bool playerIsTooCloseToWord = distanceFromWordToPlayer < distanceForChange;
        if (playerIsTooCloseToWord)
        {
            UpdateWordPosition();
        }
    }

    private void UpdateWordPosition()
    {
        List<Transform> positionsOfPlatformsInView = GetPlatformsInView();
        Transform closestPlatform = GetValidPlatformPosClosestToPlayer(positionsOfPlatformsInView); //could return null
        if (closestPlatform == null)
        {
            word.enabled = false;
        }
        else
        {
            word.enabled = true;
            PositionAbovePlatform(closestPlatform);
            RotateTowardsCamera();
        }
    }

    private List<Transform> GetPlatformsInView()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        List<Transform> platformsInView = new();
        foreach (Renderer platformRenderer in platformRenderersInScene)
        {
            if (GeometryUtility.TestPlanesAABB(frustumPlanes, platformRenderer.bounds))
            {
                platformsInView.Add(platformRenderer.transform);
            }
        }
        return platformsInView;
    }

    private Transform GetValidPlatformPosClosestToPlayer(List<Transform> platforms)
    {
        Transform platformFound = null;

        foreach (Transform platform in platforms)
        {
            //make sure its far enough from the player
            float distanceToPlayer = Vector3.Distance(playerTransform.position, platform.position);
            bool isFarEnoughFromPlayer = distanceToPlayer > minDistanceForPlatformToJumpTo;
            if (!isFarEnoughFromPlayer)
            {
                continue;
            }

            //if its the first platform we see, save it
            if (platformFound == null)
            {
                platformFound = platform;
                continue;
            }

            //check if its the closest platform to the player
            float distanceToPlayerOfSavedPos = Vector3.Distance(playerTransform.position, platformFound.position);
            bool isCloserThanSavedPlatform = distanceToPlayer < distanceToPlayerOfSavedPos;
            if (isCloserThanSavedPlatform)
            {
                platformFound = platform;
            }
        }

        return platformFound;
    }

    private void PositionAbovePlatform(Transform platform)
    {
        float platformHeight = platform.lossyScale.y;

        float wordPosX = platform.position.x;
        float wordPosY = platform.position.y + platformHeight * heightAbovePlatform;
        float wordPosZ = platform.position.z;

        transform.position = new Vector3(wordPosX, wordPosY, wordPosZ);
    }

    private void RotateTowardsCamera()
    {
        Vector3 directionVector = (transform.position - cam.transform.position).normalized;
        transform.forward = directionVector;
    }
}
