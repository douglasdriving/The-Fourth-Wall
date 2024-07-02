using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    public static void PlayNarration(AudioClip clip, SubtitleData subtitle)
    {
        //here, we could handle clip queuing etc (if it should interrupt current plays etc.)
        VoiceOverPlayer.PlayClip(clip);
        SubtitlePlayer.StartSubtitles(subtitle);
    }
}
