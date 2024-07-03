using UnityEngine;

namespace Narration
{
  public class NarrationTrigger : MonoBehaviour
  {
    public AudioClip audioClip;
    public TextAsset subtitleJson;
    SubtitleJsonData subtitle;

    void Awake()
    {
      subtitle = SubtitleJsonReader.ReadSubtitleJson(subtitleJson.text);
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        NarrationManager.PlayNarration(audioClip, subtitle);
        Destroy(gameObject);
      }
    }
  }
}
