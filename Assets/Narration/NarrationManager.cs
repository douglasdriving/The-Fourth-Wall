using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public static void PlayNarration(AudioClip clip, SubtitleJsonData subtitle)
    {
        VoiceOverPlayer.PlayClip(clip);
        SubtitlePlayer.StartSubtitles(subtitle);
    }
}
