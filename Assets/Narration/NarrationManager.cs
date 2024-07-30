using UnityEngine;

namespace Narration
{
    public class NarrationManager : MonoBehaviour
    {

        static SubtitlePlayer subtitlePlayer;

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
        }

        public static void PlayNarration(AudioClip clip, SubtitleJsonData subtitle)
        {
            VoiceOverPlayer.PlayClip(clip);
            subtitlePlayer.StartSubtitles(subtitle);
        }

        public static void StopNarration()
        {
            VoiceOverPlayer.Stop();
            subtitlePlayer.StopSubtitle();
        }
    }

}
