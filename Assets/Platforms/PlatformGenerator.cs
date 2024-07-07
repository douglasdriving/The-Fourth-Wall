using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generates the next platform in the level
public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] Vector3 levelDir = Vector3.forward;
    [SerializeField] GameObject lastInstatiatedPlatform;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject wordPointPrefab;
    [SerializeField] float maxTurnAngle = 45;
    [SerializeField] float distanceBetweenPlatforms = 8f;
    [SerializeField] float maxPlatformHeightDiff = 2;
    [SerializeField] Transform wordPointsParent;
    [SerializeField] Transform platformsParent;

    public void GenerateNextPlatform()
    {
        RandomlyRotateLevelDirection();
        Vector3 platformPos = GetNextPlatformPos();
        InstantiateRandomizedPlatform(platformPos);
        InstantiateWordPointAbovePlatform(platformPos);
    }

    private void InstantiateRandomizedPlatform(Vector3 nextPlatformPoint)
    {
        GameObject platform = Instantiate(platformPrefab, nextPlatformPoint, Quaternion.identity);
        platform.GetComponent<TransformRandomizer>().Randomize();
        platform.transform.parent = platformsParent;
        lastInstatiatedPlatform = platform;
    }

    private Vector3 GetNextPlatformPos()
    {
        Vector3 nextPlatformPoint = lastInstatiatedPlatform.transform.position + levelDir * distanceBetweenPlatforms;
        float zDiff = Random.Range(-maxPlatformHeightDiff, maxPlatformHeightDiff);
        nextPlatformPoint.z += zDiff;
        return nextPlatformPoint;
    }

    private void RandomlyRotateLevelDirection()
    {
        float turnAngle = Random.Range(-maxTurnAngle, maxTurnAngle);
        Vector3 rotVector = new Vector3(0, turnAngle, 0);
        Quaternion rot = Quaternion.Euler(rotVector);
        levelDir = rot * levelDir;
        levelDir.Normalize();
    }

    private void InstantiateWordPointAbovePlatform(Vector3 platformPos)
    {
        float heightAbovePlatform = 2f;
        Vector3 wordPos = new Vector3(platformPos.x, platformPos.y + heightAbovePlatform, platformPos.z);
        GameObject wordPoint = Instantiate(wordPointPrefab, wordPos, Quaternion.identity);
        wordPoint.transform.parent = wordPointsParent;
    }
}
