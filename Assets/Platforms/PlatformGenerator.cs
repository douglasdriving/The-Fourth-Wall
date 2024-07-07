using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//generates the next platform in the level
public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] Vector3 levelDir = Vector3.forward;
    [SerializeField] GameObject lastPlatformGenerated;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] float maxTurnAngle = 45;
    [SerializeField] float distanceBetweenPlatforms = 8f;
    [SerializeField] float maxPlatformHeightDiff = 2;

    //this should be called when the player jumps to the last platform?
    //or the last last platform
    public void GenerateNextPlatform()
    {
        RandomlyRotateLevelDirection();

        Vector3 nextPlatformPoint = lastPlatformGenerated.transform.position + levelDir * distanceBetweenPlatforms;
        float zDiff = Random.Range(-maxPlatformHeightDiff, maxPlatformHeightDiff);
        nextPlatformPoint.z += zDiff;
        GameObject platform = Instantiate(platformPrefab, nextPlatformPoint, Quaternion.identity);
        lastPlatformGenerated = platform;

        platform.GetComponent<TransformRandomizer>().Randomize();
    }

    private void RandomlyRotateLevelDirection()
    {
        float turnAngle = Random.Range(-maxTurnAngle, maxTurnAngle);
        Vector3 rotVector = new Vector3(0, turnAngle, 0);
        Quaternion rot = Quaternion.Euler(rotVector);
        levelDir = rot * levelDir;
        levelDir.Normalize();
    }
}
