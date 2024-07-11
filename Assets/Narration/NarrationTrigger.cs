using UnityEngine;

namespace Narration
{
  public class NarrationTrigger : MonoBehaviour
  {
    public AudioClip audioClip;
    public TextAsset subtitleJson;
    public SubtitleJsonData subtitle;
    bool triggered = false;

    void Awake()
    {
      subtitle = SubtitleJsonReader.ReadSubtitleJson(subtitleJson.text);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (triggered) return;
      if (!other.CompareTag("Player")) return;

      NarrationManager.PlayNarration(audioClip, subtitle);
      triggered = true;
    }
  }
}
