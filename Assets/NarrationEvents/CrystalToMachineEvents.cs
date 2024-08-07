using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalToMachineEvents : MonoBehaviour
{
    [SerializeField] float timeForMachinePlatformSpawn = 82.38f;
    [SerializeField] GameObject machinePlaftormPrefab;
    [SerializeField] float gapFromWalkwayToMachinePlatform = 10f;

    bool eventRunning = false;
    bool eventFinished = false;
    bool spawnedMachinePlatform = false;
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

        if (spawnedMachinePlatform) return;
        if (timeEventHasBeenRunning < timeForMachinePlatformSpawn) return;

        SpawnMachinePlatform();
        EndEvent();
    }

    private void SpawnMachinePlatform()
    {
        levelGenerator.SpawnCustomPlatform(machinePlaftormPrefab, gapFromWalkwayToMachinePlatform);
        spawnedMachinePlatform = true;
    }

    private void EndEvent()
    {
        eventRunning = false;
        eventFinished = true;
    }

}
