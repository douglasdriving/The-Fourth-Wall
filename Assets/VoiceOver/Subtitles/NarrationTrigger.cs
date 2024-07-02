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
    //text code:
    SubtitlePlayer.StartSubtitles(subtitleData); //delete me
  }

  void LoadSubtitleData()
  {
    subtitleData = JsonUtility.FromJson<SubtitleData>(subtitles.text);

    //debugging stuff
    // string audioClipName = subtitleData.audioClipName;
    // WordTimestamp firstWord = subtitleData.sentences[0].words[0];
    // string firstWordText = subtitleData.sentences[0].words[0].word;
    // float firstWordTime = subtitleData.sentences[0].words[0].time;
    // Debug.Log("subtitle data for clip: " + audioClipName + " loaded. First word is: " + firstWordText + " at time: " + firstWordTime);
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
