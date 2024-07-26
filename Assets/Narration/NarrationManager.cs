using UnityEngine;

namespace Narration
{
    public class NarrationManager : MonoBehaviour
    {
        public static void PlayNarration(AudioClip clip, SubtitleJsonData subtitle)
        {
            VoiceOverPlayer.PlayClip(clip);
            FindObjectOfType<SubtitlePlayer>().StartSubtitles(subtitle);
        }
    }

}
