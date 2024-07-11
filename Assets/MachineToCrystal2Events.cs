using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

public class MachineToCrystal2Events : MonoBehaviour
{
    //starts the events from the machine over to crystal 2
    [SerializeField] Transform endOfExitPlatform;
    bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!activated) return;

        activated = true;

        Vector3 pathStart = endOfExitPlatform.position;
        Vector3 pathEnd = GameObject.FindWithTag("SecondCrystalPlatformEntryPoint").transform.position;
        SubtitleJsonData subtitle = GetComponent<NarrationTrigger>().subtitle;
        int subtitleWordCount = SubtitleJsonReader.CountWordsInSubtitle(subtitle);
        int totalPlatformPieceCount = subtitleWordCount;
        FindObjectOfType<LevelGenerator>().SetPlatformingPath(pathStart, pathEnd, totalPlatformPieceCount);
    }
}
