using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

/// <summary>
/// trigger narration from c3 to machine
/// enables superspeed at the right timing
/// put on crystal3
/// </summary>
public class Crystal3ToMachineEvents : MonoBehaviour
{
    [SerializeField] AudioClip c3ToMachineAudio;
    [SerializeField] TextAsset c3ToMachineSubtitle;
    [SerializeField] float timeToEnableSuperSpeed = 30.82f;
    [SerializeField] float superspeedVelocity = 23f;

    public void TriggerEvents()
    {
        SetBackwardSubtitleSpawnFromLastLevelPiece();
        NarrationManager.PlayNarration(c3ToMachineAudio, c3ToMachineSubtitle);
        StartCoroutine(EnableSuperSpeedAfterTimer());
    }

    private static void SetBackwardSubtitleSpawnFromLastLevelPiece()
    {
        int indexOfLastLevelPiece = FindObjectOfType<LevelGenerator>().levelPiecesSpawned.Count - 1;
        FindObjectOfType<SubtitlePlayer>().SetMode(SubtitleMode.SpawnBackwardOnLevel, indexOfLastLevelPiece);
    }

    IEnumerator EnableSuperSpeedAfterTimer()
    {
        yield return new WaitForSeconds(timeToEnableSuperSpeed);
        FirstPersonController playerController = FindObjectOfType<FirstPersonController>();
        playerController.enableSprint = true;
        playerController.sprintSpeed = superspeedVelocity;
        Debug.Log("SuperSpeed enabled");
    }
}
