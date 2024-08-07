using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

public class MachineToCrystal2Events : MonoBehaviour
{
    [SerializeField] Transform endOfExitPlatform;
    [SerializeField] AudioClip audioClip;
    [SerializeField] TextAsset subtitleJson;
    SubtitleJsonData subtitle;
    bool activated = false;

    void Awake()
    {
        subtitle = SubtitleJsonReader.ReadSubtitleJson(subtitleJson.text);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (activated) return;

        activated = true;

        Vector3 pathStart = endOfExitPlatform.position;
        Vector3 pathEnd = GameObject.FindWithTag("SecondCrystalPlatformEntryPoint").transform.position;
        int subtitleWordCount = SubtitleJsonReader.CountWordsInSubtitle(subtitle);
        int totalPlatformPieceCount = subtitleWordCount;

        FindObjectOfType<LevelGenerator>().SetPlatformingPath(pathStart, pathEnd, totalPlatformPieceCount);
        NarrationManager.PlayNarration(audioClip, subtitle);
    }
}
