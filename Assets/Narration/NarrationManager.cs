using System.Collections.Generic;
using LevelGeneration;
using UnityEngine;

namespace Narration
{
    /// <summary>
    /// manages the playing of narration clips and subtitles
    /// </summary>
    public class NarrationManager : MonoBehaviour
    {

        public enum PlayState
        {
            STOP,
            PLAY,
            PAUSE
        }

        const int numberOfUnpausePiecesOnPause = 8;
        const float pauseDelay = 0.12f;

        static SubtitlePlayer subtitlePlayer;
        public static PlayState playState = PlayState.STOP;
        public static float timeCurrentNarrationHasPlayed = 0;
        static float clipLength = 0;
        static List<float> pausesScheduled = new();

        [SerializeField] AudioClip clipToPlayOnStart;
        [SerializeField] TextAsset subtitleToPlayOnStart;
        [SerializeField] float startDelay = 1.5f;

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
        }

        void Start()
        {
            Invoke("PlayStartNarration", startDelay);
        }

        void PlayStartNarration()
        {
            PlayNarration(clipToPlayOnStart, subtitleToPlayOnStart);
            StartCoroutine(FindAnyObjectByType<ExitPortalGenerator>().GenerateExitPortalAfterDelay(clipToPlayOnStart.length));
        }

        public static void PlayNarration(AudioClip clip, TextAsset subtitle)
        {
            SubtitleJsonData subtitleJsonData = SubtitleJsonReader.ReadSubtitleJson(subtitle.text);
            PlayNarration(clip, subtitleJsonData);
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

            if (pausesScheduled.Count > 0 && timeCurrentNarrationHasPlayed > pausesScheduled[0])
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
