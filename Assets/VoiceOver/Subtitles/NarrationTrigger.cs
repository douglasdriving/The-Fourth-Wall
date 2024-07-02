using System.Collections.Generic;
using UnityEngine;

public class NarrationTrigger : MonoBehaviour
{
  public AudioClip audioClip;
  public TextAsset subtitles;
  private SubtitleData subtitleData;

  void Start()
  {
    LoadSubtitleData();
  }

  void LoadSubtitleData()
  {
    subtitleData = JsonUtility.FromJson<SubtitleData>(subtitles.text);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      NarrationManager.PlayNarration(audioClip, subtitleData);
      Destroy(gameObject);
    }
  }
}
