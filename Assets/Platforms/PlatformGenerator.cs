using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generates the next platform in the level
//i think we could have a base class for the generator that both this and walkway generator inherits from.
public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject wordPointPrefab;
    [SerializeField] float maxTurnAngle = 45;
    [SerializeField] float distanceBetweenPlatforms = 8f;
    [SerializeField] float maxPlatformHeightDiff = 2;
    [SerializeField] Transform wordPointsParent;
    [SerializeField] Transform platformsParent;

    public GameObject GenerateNextPlatform(Vector3 pointToMoveFrom)
    {
        Vector3 platformPos = GetNextPlatformPos(pointToMoveFrom);
        GameObject platform = InstantiateRandomizedPlatform(platformPos);
        InstantiateWordPointAbovePlatform(platformPos);
        return platform;
    }

    private Vector3 GetNextPlatformPos(Vector3 pointToMoveFrom)
    {
        Vector3 directionToMoveIn = new Vector3(Random.Range(-1, 1), 0, Random.value).normalized;
        Vector3 nextPlatformPoint = pointToMoveFrom + directionToMoveIn * distanceBetweenPlatforms;
        float zDiff = Random.Range(-maxPlatformHeightDiff, maxPlatformHeightDiff);
        nextPlatformPoint.z += zDiff;
        return nextPlatformPoint;
    }

    private GameObject InstantiateRandomizedPlatform(Vector3 nextPlatformPoint)
    {
        GameObject platform = Instantiate(platformPrefab, nextPlatformPoint, Quaternion.identity);
        platform.GetComponent<TransformRandomizer>().Randomize();
        platform.transform.parent = platformsParent;
        return platform;
    }

    private void InstantiateWordPointAbovePlatform(Vector3 platformPos)
    {
        float heightAbovePlatform = 2f;
        Vector3 wordPos = new Vector3(platformPos.x, platformPos.y + heightAbovePlatform, platformPos.z);
        GameObject wordPoint = Instantiate(wordPointPrefab, wordPos, Quaternion.identity);
        wordPoint.transform.parent = wordPointsParent;
    }
}
