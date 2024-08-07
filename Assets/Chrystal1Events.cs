using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chrystal1Events : MonoBehaviour
{

    [SerializeField] float timeForPlatformSwitch = 48;
    [SerializeField] float timeForCrystalSpawn = 51.52f;
    [SerializeField] float distanceToPlatformTwoAndThreeFromPlayerAtSpawn = 500f;

    [SerializeField] GameObject crystal1PlatformPrefab;
    [SerializeField] GameObject crystal2PlatformPrefab;
    [SerializeField] GameObject crystal3PlatformPrefab;
    [SerializeField] float gapFromWalkwayToCrystalPlatform = 15f;

    bool switchedToPlatformSpawn = false;
    bool spawnedCrystalPlatforms = false;

    bool eventRunning = false;
    bool eventFinished = false;
    float timeEventHasBeenRunning = 0;

    LevelGenerator levelGenerator;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (eventRunning) return;
        if (eventFinished) return;
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
        eventRunning = true;
    }

    void Update()
    {
        if (!eventRunning) return;
        if (eventFinished) return;

        timeEventHasBeenRunning += Time.deltaTime;

        if (!switchedToPlatformSpawn && timeEventHasBeenRunning >= timeForPlatformSwitch)
        {
            SwitchFromGeneratingWalkwayToPlatforms();
        }

        if (!spawnedCrystalPlatforms && timeEventHasBeenRunning >= timeForCrystalSpawn)
        {
            SpawnThreeCrystalPlatforms();
            EndEvent();
        }

        // if (!spawnedMachinePlatform && timeEventHasBeenRunning >= timeForMachinePlatformSpawn)
        // {
        //     SpawnMachinePlatform();
        //     EndEvent();
        // }
    }

    private void SwitchFromGeneratingWalkwayToPlatforms()
    {
        switchedToPlatformSpawn = true;
        LevelGenerator.pieceTypeBeingGenerated = LevelPieceType.PLATFORM;
    }

    private void EndEvent()
    {
        eventRunning = false;
        eventFinished = true;
    }

    private void SpawnThreeCrystalPlatforms()
    {
        SpawnFirstCrystalPlatform();
        SpawnSecondAndThirdCrystalPlatforms();
        spawnedCrystalPlatforms = true;
    }


    private void SpawnFirstCrystalPlatform()
    {
        GameObject firstCrystalPlatform = levelGenerator.SpawnCustomPlatform(crystal1PlatformPrefab, gapFromWalkwayToCrystalPlatform);
        GameObject firstCrystalPlatformWalkwayPiece = FindChildByTag(firstCrystalPlatform, "Walkway");
        levelGenerator.levelPiecesSpawned.Add(firstCrystalPlatformWalkwayPiece);
    }

    private GameObject FindChildByTag(GameObject parent, string tag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private void SpawnSecondAndThirdCrystalPlatforms()
    {
        Vector3 vectorFromPlayerToCrystalPlatform2 = new Vector3(-0.5f, 0, 1).normalized * distanceToPlatformTwoAndThreeFromPlayerAtSpawn;
        Vector3 vectorFromPlayerToCrystalPlatform3 = new Vector3(0.5f, 0, 1).normalized * distanceToPlatformTwoAndThreeFromPlayerAtSpawn;

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;

        Vector3 crystal2PlatformPos = playerPos + vectorFromPlayerToCrystalPlatform2;
        Vector3 crystal3PlatformPos = playerPos + vectorFromPlayerToCrystalPlatform3;

        GameObject crystal2Platform = Instantiate(crystal2PlatformPrefab, crystal2PlatformPos, Quaternion.identity);
        GameObject crystal3Platform = Instantiate(crystal3PlatformPrefab, crystal3PlatformPos, Quaternion.identity);

        crystal2Platform.GetComponentInChildren<TMP_Text>().text = "Crystal 2";
        crystal3Platform.GetComponentInChildren<TMP_Text>().text = "Crystal 3";
    }
}
