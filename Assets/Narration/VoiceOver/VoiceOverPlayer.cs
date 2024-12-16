using System.Collections.Generic;
using UnityEngine;

namespace Narration
{
    /// <summary>
    /// Plays voice over clips
    /// </summary>
    public class VoiceOverPlayer : MonoBehaviour
    {
        private static AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public static void PlayClip(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.volume = GetNormalizedVolume(clip);
            audioSource.Play();
        }

        private static float GetNormalizedVolume(AudioClip clip)
        {
            float maxVolume = 1.0f;
            float minVolume = 0f;
            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            List<float> peakSamples = new List<float>();
            for (int i = 0; i < samples.Length; i++)
            {
                peakSamples.Add(Mathf.Abs(samples[i]));
            }
            peakSamples.Sort((a, b) => b.CompareTo(a)); // Sort in descending order
            int peakCount = Mathf.Min(1000, peakSamples.Count); // Take the top 1000 samples or less if there are not enough samples
            float sum = 0f;
            for (int i = 0; i < peakCount; i++)
            {
                sum += peakSamples[i];
            }
            float averagePeakVolume = sum / peakCount;
            float desiredAveragePeakVolume = 0.3f;
            float playbackVolume = desiredAveragePeakVolume / averagePeakVolume;
            playbackVolume = Mathf.Clamp(playbackVolume, minVolume, maxVolume);
            return playbackVolume;
        }

        public static void Play()
        {
            audioSource.Play();
        }

        public static void Stop()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        public static void Pause()
        {
            audioSource.Pause();
        }
    }
}

