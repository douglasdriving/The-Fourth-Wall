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
            audioSource.Play();
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

