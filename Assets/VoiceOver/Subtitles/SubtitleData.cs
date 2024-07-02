using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubtitleData
{
  public string audioClipName;
  public List<SentenceData> sentences;
}

[System.Serializable]
public class SentenceData
{
  public List<WordTimestamp> words;
}

[System.Serializable]
public class WordTimestamp
{
  public string word;
  public float time;
}
