using System.Collections.Generic;
using Narration;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    Vector3 savedPlayerPos;
    Quaternion savedPlayerRot;
    GameObject player;

    int indexOfLastSavedPlatform = 0;
    LevelGenerator levelGenerator;

    GameRules savedRules;

    List<NarrationTrigger> triggersEnteredAfterSave = new();

    void Awake()
    {
        player = FindObjectOfType<FirstPersonController>().gameObject;
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
    }

    void OnEnabled()
    {
        NarrationTrigger.OnNarrationTriggerEntered += AddNarrationTriggersEnteredAfterSave;
    }

    void OnDisable()
    {
        NarrationTrigger.OnNarrationTriggerEntered -= AddNarrationTriggersEnteredAfterSave;
    }

    private void AddNarrationTriggersEnteredAfterSave(NarrationTrigger trigger)
    {
        triggersEnteredAfterSave.Add(trigger);
    }

    public void SaveCurrentState()
    {
        savedPlayerPos = player.transform.position;
        savedPlayerRot = player.transform.rotation;

        indexOfLastSavedPlatform = levelGenerator.levelPiecesSpawned.Count - 1;

        savedRules = new GameRules(CurrentGameRules.currentGameRules);
    }

    public void KillPlayerAndReset()
    {
        player.transform.position = savedPlayerPos;
        player.transform.rotation = savedPlayerRot;

        levelGenerator.DestroyAllPiecesAboveIndex(indexOfLastSavedPlatform);

        CurrentGameRules.currentGameRules = new GameRules(savedRules);

        NarrationManager.StopNarration();

        foreach (NarrationTrigger trigger in triggersEnteredAfterSave)
        {
            trigger.triggered = false;
        }
        triggersEnteredAfterSave.Clear();
    }

}
