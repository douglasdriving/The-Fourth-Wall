using UnityEngine;

namespace Narration
{
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

        public static void Stop()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }
}

