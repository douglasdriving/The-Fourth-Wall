using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
  public class NarrationTrigger : MonoBehaviour
  {
    public AudioClip audioClip;
    public TextAsset subtitleJson;
    public SubtitleJsonData subtitle;
    public bool triggered = false;

    public delegate void NarrationTriggerEntered(NarrationTrigger trigger);
    public static event NarrationTriggerEntered OnNarrationTriggerEntered;

    [SerializeField] UnityEvent eventsToTrigger = new();

    void Awake()
    {
      subtitle = SubtitleJsonReader.ReadSubtitleJson(subtitleJson.text);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (triggered) return;
      if (!other.CompareTag("Player")) return;

      NarrationManager.PlayNarration(audioClip, subtitle);
      eventsToTrigger?.Invoke();
      triggered = true;
      OnNarrationTriggerEntered?.Invoke(this);
    }
  }
}
