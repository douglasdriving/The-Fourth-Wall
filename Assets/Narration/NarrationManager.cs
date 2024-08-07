using System.Collections.Generic;
using UnityEngine;

namespace Narration
{

    public enum PlayState
    {
        STOP,
        PLAY,
        PAUSE
    }

    public class NarrationManager : MonoBehaviour
    {

        const int numberOfUnpausePiecesOnPause = 8;
        const float pauseDelay = 0.12f;

        static SubtitlePlayer subtitlePlayer;
        public static PlayState playState = PlayState.STOP;
        public static float timeCurrentNarrationHasPlayed = 0;
        static float clipLength = 0;
        static List<float> pausesScheduled = new();

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
        }

        public static void PlayNarration(AudioClip clip, SubtitleJsonData subtitle)
        {
            VoiceOverPlayer.PlayClip(clip);
            subtitlePlayer.StartSubtitles(subtitle);
            playState = PlayState.PLAY;
            timeCurrentNarrationHasPlayed = 0;
            clipLength = clip.length;
            SetPausesFromSubtitle(subtitle);
        }

        private static void SetPausesFromSubtitle(SubtitleJsonData subtitle)
        {
            pausesScheduled.Clear();
            foreach (SubtitleWord word in subtitle.GetWords())
            {
                if (word.pause)
                {
                    pausesScheduled.Add(word.end + pauseDelay);
                }
            }
        }

        void Update()
        {
            if (playState != PlayState.PLAY) return;

            timeCurrentNarrationHasPlayed += Time.deltaTime;

            if (timeCurrentNarrationHasPlayed > pausesScheduled[0])
            {
                Pause();
                pausesScheduled.RemoveAt(0);
            }

            if (timeCurrentNarrationHasPlayed > clipLength)
            {
                StopAndReset();
            }
        }

        private static void Pause()
        {
            playState = PlayState.PAUSE;
            VoiceOverPlayer.Pause();
            UnpauseTriggerActivator.ActivateUnpauseTriggerOnLastPieces(numberOfUnpausePiecesOnPause);
        }

        public static void Unpause()
        {
            playState = PlayState.PLAY;
            VoiceOverPlayer.Play();
        }

        public static void StopAndReset()
        {
            VoiceOverPlayer.Stop();
            subtitlePlayer.StopSubtitle();
            playState = PlayState.STOP;
            timeCurrentNarrationHasPlayed = 0;
            pausesScheduled.Clear();
            clipLength = 0;
        }
    }

}
