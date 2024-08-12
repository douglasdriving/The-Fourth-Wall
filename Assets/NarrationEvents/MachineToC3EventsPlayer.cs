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
        //test

        ///real
        // timedEvents.Add(new TimedEvent(7.4f, () => CurrentGameRules.SetDangerousLetter('P')));
        // timedEvents.Add(new TimedEvent(22.88f, () => CurrentGameRules.SetDangerousLetter('G')));
        // timedEvents.Add(new TimedEvent(35.56f, () => CurrentGameRules.SetWallSpawningDirection(Direction.BACKWARD)));
        // timedEvents.Add(new TimedEvent(43.8f, () => CurrentGameRules.SetWallSpawningDirection(Direction.LEFT)));
        // timedEvents.Add(new TimedEvent(45.74f, () => CurrentGameRules.SetDangerousLetter('L')));
        // timedEvents.Add(new TimedEvent(60.14f, () => CurrentGameRules.SetDissapearingPlatformsWithKey(Keyboard.current.kKey)));
        // timedEvents.Add(new TimedEvent(63.18f, () => CurrentGameRules.SetDangerousLetter('K')));
        // timedEvents.Add(new TimedEvent(73.22f, () => CurrentGameRules.SetDissapearingPlatformsWithKey(Keyboard.current.lKey)));

        /// 1. turn off the lights
        BlackCoverCanvas blackCoverCanvas = FindObjectOfType<BlackCoverCanvas>();
        timedEvents.Add(new TimedEvent(7.4f, () => blackCoverCanvas.SetCoverActive(true)));
        timedEvents.Add(new TimedEvent(10.4f, () => blackCoverCanvas.SetPlayerToggleActive(true)));

        /// 2. remove platforms behind and ahead of player
        /// 3. make the player stuck
        /// 4. spawn a destructable wall
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
