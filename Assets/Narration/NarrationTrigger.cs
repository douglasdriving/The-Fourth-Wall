using UnityEngine;

namespace Narration
{

  public enum SubtitleMode
  {
    SpawnWithNewLevelPiece,
    SpawnForwardOnLevel,
    SpawnBackwardOnLevel
  }

  public class NarrationTrigger : MonoBehaviour
  {
    [SerializeField] SubtitleMode subtitleMode = SubtitleMode.SpawnWithNewLevelPiece;
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
