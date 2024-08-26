using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Triggers an event after a certain amount of time has passed since the trigger event
/// </summary>
public class TriggerEventTimer : MonoBehaviour
{

    [SerializeField] string tagToTriggerFor = "Player";
    [SerializeField] bool onlyTriggerOnce = true;
    [SerializeField] float timeBeforeEvent = 30;
    [SerializeField] UnityEvent eventToToFireAfterTimer;
    bool hasBeenTriggeredAtLeastOnce = false;
    float timeSinceTrigger = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tagToTriggerFor)) return;
        if (hasBeenTriggeredAtLeastOnce && onlyTriggerOnce) return;
        if (hasBeenTriggeredAtLeastOnce && IsTimerRunning()) return;

        hasBeenTriggeredAtLeastOnce = true;
        timeSinceTrigger = timeBeforeEvent;
    }

    void Update()
    {
        if (IsTimerRunning())
        {
            timeSinceTrigger -= Time.deltaTime;
            bool timerDone = timeSinceTrigger <= 0;
            if (timerDone)
            {
                timeSinceTrigger = -1;
                eventToToFireAfterTimer.Invoke();
            }
        }
    }

    bool IsTimerRunning()
    {
        return hasBeenTriggeredAtLeastOnce && timeSinceTrigger > 0;
    }
}
