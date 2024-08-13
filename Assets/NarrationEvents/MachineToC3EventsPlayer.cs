using System.Collections.Generic;
using Narration;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// starts the narration event chain from the machine platform to the crystal 3 platform when the player enters the trigger volume 
/// </summary>
public class MachineToC3EventsPlayer : MonoBehaviour
{
    bool isRunning = false;
    float timeRan = 0;

    class TimedEvent
    {
        public float timestamp;
        public bool hasFired;
        public System.Action eventAction;

        public TimedEvent(float timestamp, System.Action eventAction)
        {
            this.timestamp = timestamp;
            this.eventAction = eventAction;
            this.hasFired = false;
        }
    }

    private List<TimedEvent> timedEvents = new List<TimedEvent>();

    private void Start()
    {
        BlackCoverCanvas blackCoverCanvas = FindObjectOfType<BlackCoverCanvas>();
        PlatformToggler platformToggler = FindObjectOfType<PlatformToggler>();
        PlayerMoveHolder playerMoveHolder = FindObjectOfType<PlayerMoveHolder>();
        ObjectOnLevelPieceSpawner objectOnLevelPieceSpawner = FindObjectOfType<ObjectOnLevelPieceSpawner>();

        //test

        /// 1. turn off the lights
        timedEvents.Add(new TimedEvent(14.0f, () => blackCoverCanvas.SetCoverActive(true)));
        timedEvents.Add(new TimedEvent(26.34f, () => blackCoverCanvas.SetPlayerToggleActive(true)));

        /// 2. remove platforms behind and ahead of player
        timedEvents.Add(new TimedEvent(33.62f, () => platformToggler.PopAllPlatformsExceptTheOnePlayerIsOn()));
        timedEvents.Add(new TimedEvent(44.33f, () => platformToggler.PopAllPlatformsExceptTheOnePlayerIsOn()));

        /// 3. make the player stuck
        timedEvents.Add(new TimedEvent(50.98f, () => playerMoveHolder.HoldPlayer()));

        /// 4. spawn a destructable wall
        timedEvents.Add(new TimedEvent(71.32f, () => objectOnLevelPieceSpawner.isSpawningOnNextPiece = true));
    }

    public void StartEventChain()
    {
        isRunning = true;
    }

    void Update()
    {
        if (!isRunning) return;
        if (NarrationManager.playState != PlayState.PLAY) return;

        bool allEventsFired = true;

        foreach (var timedEvent in timedEvents)
        {
            if (!timedEvent.hasFired && timeRan >= timedEvent.timestamp)
            {
                timedEvent.eventAction.Invoke();
                timedEvent.hasFired = true;
            }

            if (!timedEvent.hasFired)
            {
                allEventsFired = false;
            }
        }

        if (allEventsFired)
        {
            isRunning = false;
        }
        else
        {
            timeRan += Time.deltaTime;
        }
    }
}
