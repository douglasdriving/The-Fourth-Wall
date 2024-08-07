using System.Collections.Generic;
using Narration;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    static Vector3 savedPlayerPos;
    static Quaternion savedPlayerRot;
    static GameObject player;

    static int indexOfLastSavedPlatform = 0;
    static LevelGenerator levelGenerator;

    static GameRules savedRules;

    static List<NarrationTrigger> triggersEnteredAfterSave = new();

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDied;

    void Awake()
    {
        player = FindObjectOfType<FirstPersonController>().gameObject;
        levelGenerator = FindAnyObjectByType<LevelGenerator>();
        SaveCurrentState();
    }

    void OnEnable()
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

    public static void SaveCurrentState()
    {
        savedPlayerPos = player.transform.position;
        savedPlayerRot = player.transform.rotation;

        indexOfLastSavedPlatform = levelGenerator.levelPiecesSpawned.Count - 1;

        savedRules = new GameRules(CurrentGameRules.rules);
    }

    public static void KillPlayerAndReset()
    {
        player.transform.position = savedPlayerPos;
        player.transform.rotation = savedPlayerRot;

        levelGenerator.DestroyAllPiecesAboveIndex(indexOfLastSavedPlatform);

        CurrentGameRules.rules = new GameRules(savedRules);

        NarrationManager.StopNarration();

        foreach (NarrationTrigger trigger in triggersEnteredAfterSave)
        {
            trigger.triggered = false;
        }
        triggersEnteredAfterSave.Clear();

        OnPlayerDied?.Invoke();
    }

}
