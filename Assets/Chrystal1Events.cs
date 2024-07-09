using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chrystal1Events : MonoBehaviour
{

    [SerializeField] float timeForPlatformSwitch = 48;
    [SerializeField] float timeForCrystalSpawn = 51.52f;
    [SerializeField] float timeForMachinePlatformSpawn = 82.38f;

    [SerializeField] GameObject crystalPlatformPrefab;
    [SerializeField] GameObject machinePlaftormPrefab;

    bool switchedToPlatformSpawn = false;
    bool spawnedCrystal = false;
    bool spawnedMachinePlatform = false;

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
            switchedToPlatformSpawn = true;
            LevelGenerator.pieceTypeBeingGenerated = LevelPieceType.PLATFORM;
        }

        if (!spawnedCrystal && timeEventHasBeenRunning >= timeForCrystalSpawn)
        {
            levelGenerator.SpawnCustomPlatform(crystalPlatformPrefab);
            spawnedCrystal = true;
        }

        if (!spawnedMachinePlatform && timeEventHasBeenRunning >= timeForMachinePlatformSpawn)
        {
            levelGenerator.SpawnCustomPlatform(machinePlaftormPrefab);
            spawnedMachinePlatform = true;
            eventRunning = false;
            eventFinished = true;
        }
    }
}
