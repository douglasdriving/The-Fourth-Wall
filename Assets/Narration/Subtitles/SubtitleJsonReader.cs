using UnityEngine;

namespace Narration
{
    public class SubtitleJsonReader : MonoBehaviour
    {
        public static SubtitleJsonData ReadSubtitleJson(string subtitleJsonString)
        {
            SubtitleJsonData data = JsonUtility.FromJson<SubtitleJsonData>(subtitleJsonString);
            return data;
        }
    }

    [System.Serializable]
    public class SubtitleJsonData
    {
        public string text;
        public SubtitleSegment[] segments;
        public string language;
    }

    [System.Serializable]
    public class SubtitleSegment
    {
        public int id;
        public int seek;
        public float start;
        public float end;
        public string text;
        public int[] tokens;
        public float temperature;
        public float avg_logprob;
        public float compression_ratio;
        public float no_speech_prob;
        public SubtitleWord[] words;
    }

    [System.Serializable]
    public class SubtitleWord
    {
        public string word;
        public float start;
        public float end;
        public float probability;
    }
}
