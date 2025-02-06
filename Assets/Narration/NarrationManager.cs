using System.Collections;
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
        public static float timePlayed = 0;
        static float totalDuration = 0;
        static List<float> pausesScheduled = new();

        [SerializeField] AudioClip clipToPlay;
        [SerializeField] TextAsset subtitleToPlayOnStart;
        [SerializeField] float startDelay = 1.5f;
        [SerializeField] bool endSceneOnEnd = false;
        [SerializeField] string placeHolderText = "This is the placeholder devlog text. It should be replaced with a proper description of the class.";

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
        }

        void Start()
        {
            Invoke("PlayNarration", startDelay);
        }

        void PlayNarration()
        {

            SubtitleJsonData subtitle;

            if (subtitleToPlayOnStart == null)
            {
                Debug.LogWarning("No subtitle file found for narration clip. Will run placeholder script.");
                subtitle = SubtitleJsonReader.MakeSubtitleFromText(placeHolderText);
                totalDuration = subtitle.GetTotalDuration();
            }
            else
            {
                VoiceOverPlayer.PlayClip(clipToPlay);
                subtitle = SubtitleJsonReader.ReadSubtitleJson(subtitleToPlayOnStart.text);
                totalDuration = clipToPlay.length;
            }

            if (subtitlePlayer == null) Debug.LogWarning("SubtitlePlayer not found in scene, will not start subtitles");
            else subtitlePlayer.StartSubtitles(subtitle);

            playState = PlayState.PLAY;
            timePlayed = 0;
            SetPausesFromSubtitle(subtitle);
            SchedulePortalSpawn(totalDuration);
            if (endSceneOnEnd) StartCoroutine(EndSceneAfterDelay(totalDuration));

        }

        IEnumerator EndSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneTransitioner sceneTransitioner = FindObjectOfType<SceneTransitioner>();
            if (sceneTransitioner != null) sceneTransitioner.EndScene();
            else Debug.LogWarning("SceneTransitioner not found in scene. Will not attempt to end scene.");
        }

        private void SchedulePortalSpawn(float timeBeforeSpawn) //should this really happen in this class?
        {
            ExitPortalGenerator exitPortalGenerator = FindObjectOfType<ExitPortalGenerator>();
            if (exitPortalGenerator != null) StartCoroutine(exitPortalGenerator.GenerateExitPortalAfterDelay(timeBeforeSpawn));
            else Debug.LogWarning("ExitPortalGenerator not found in scene. Will not attempt to spawn exit portal.");
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

            timePlayed += Time.deltaTime;

            if (pausesScheduled.Count > 0 && timePlayed > pausesScheduled[0])
            {
                Pause();
                pausesScheduled.RemoveAt(0);
            }

            if (timePlayed > totalDuration)
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
            if (subtitlePlayer != null) subtitlePlayer.StopSubtitle();
            playState = PlayState.STOP;
            timePlayed = 0;
            pausesScheduled.Clear();
            totalDuration = 0;
        }
    }

}
