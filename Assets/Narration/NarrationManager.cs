using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
using Player;
using UnityEngine;
using UnityEngine.Video;

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

        const float pauseDelay = 0.1f;

        static SubtitlePlayer subtitlePlayer;
        public static PlayState playState = PlayState.STOP;
        public static float timePlayed = 0;
        static float totalDuration = 0;
        static List<float> pausesScheduled = new();

        [SerializeField] AudioClip audioClip;
        [SerializeField] VideoClip videoClip;
        [SerializeField] TextAsset subtitle;
        [SerializeField] bool endSceneOnEnd = false;
        [SerializeField] string placeHolderText = "This is the placeholder devlog text. It should be replaced with a proper description of the class.";
        [SerializeField] VideoPlayer videoPlayer;
        WalkwayDistanceChecker playerWalkwayDistanceChecker;
        ExitPortalGenerator exitPortalGenerator;
        PauseMenu pauseMenu;

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
            playerWalkwayDistanceChecker = FindObjectOfType<WalkwayDistanceChecker>();
            exitPortalGenerator = FindObjectOfType<ExitPortalGenerator>();
            pauseMenu = FindObjectOfType<PauseMenu>();

            if (videoClip != null && audioClip != null)
            {
                Debug.LogWarning("both audio and video provided. will only play video.");
            }
        }

        public void PlayNarration()
        {

            SubtitleJsonData subtitleData;

            if (subtitle == null)
            {
                Debug.LogWarning("No subtitle file found for narration clip. Will run placeholder script.");
                subtitleData = SubtitleJsonReader.MakeSubtitleFromText(placeHolderText);
                totalDuration = subtitleData.GetTotalDuration();
            }
            else
            {
                if (videoClip)
                {
                    videoPlayer.clip = videoClip;
                    videoPlayer.Play();
                    totalDuration = (float)videoClip.length;
                }
                else
                {
                    VoiceOverPlayer.PlayClip(audioClip);
                    totalDuration = audioClip.length;
                }
                subtitleData = SubtitleJsonReader.ReadSubtitleJson(subtitle.text);
            }

            if (subtitlePlayer == null) Debug.LogWarning("SubtitlePlayer not found in scene, will not start subtitles");
            else subtitlePlayer.StartSubtitles(subtitleData);

            playState = PlayState.PLAY;
            timePlayed = 0;

            SceneRules rules = FindObjectOfType<SceneRules>();
            if (rules != null && rules.pauseBetweenSentences)
            {
                SchedulePausesAfterEachSentence(subtitleData);
            }

            SchedulePortalSpawn(totalDuration);
            if (endSceneOnEnd) StartCoroutine(EndSceneAfterDelay(totalDuration));

        }

        private static void SchedulePausesAfterEachSentence(SubtitleJsonData subtitleData)
        {
            pausesScheduled.Clear();
            if (subtitleData != null)
            {
                foreach (SubtitleSegment segment in subtitleData.segments)
                {
                    foreach (SubtitleWord word in segment.words)
                    {
                        string wordText = word.word;
                        bool endsSentence = wordText.EndsWith(".") || wordText.EndsWith("!") || wordText.EndsWith("?");
                        if (endsSentence)
                        {
                            float pauseTime = word.end + pauseDelay;
                            pausesScheduled.Add(pauseTime);
                        }
                    }
                }
            }
        }

        IEnumerator EndSceneAfterDelay(float delay)
        {
            float elapsed = 0f;

            while (elapsed < delay)
            {
                if (playState == PlayState.PLAY)
                {
                    elapsed += Time.deltaTime;
                }
                yield return null; // Wait for the next frame
            }

            SceneTransitioner sceneTransitioner = FindObjectOfType<SceneTransitioner>();
            if (sceneTransitioner != null)
                sceneTransitioner.EndScene();
            else
                Debug.LogWarning("SceneTransitioner not found in scene. Will not attempt to end scene.");
        }

        private void SchedulePortalSpawn(float timeBeforeSpawn)
        {
            if (exitPortalGenerator != null) exitPortalGenerator.StartSpawnCountdown(timeBeforeSpawn);
            else Debug.LogWarning("ExitPortalGenerator not found in scene. Will not attempt to spawn exit portal.");
        }

        void Update()
        {
            if (playState == PlayState.PLAY)
            {
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
            else if (playState == PlayState.PAUSE)
            {
                ResumeIfPlayerIsCloseToEnd();
            }
        }

        private void ResumeIfPlayerIsCloseToEnd()
        {
            if (pauseMenu != null && pauseMenu.CheckIsPaused())
            {
                return; // if the pause menu is open, we dont want to resume the narration
            }
            float distanceToWalkoffPoint = playerWalkwayDistanceChecker.GetDistanceToWalkwayEnd();
            if (distanceToWalkoffPoint < 10)
            {
                Resume();
            }
        }

        public void Pause()
        {
            playState = PlayState.PAUSE;
            if (videoClip) videoPlayer.Pause();
            else VoiceOverPlayer.Pause();
            exitPortalGenerator.PauseSpawnCountdown();
        }

        public void Resume()
        {
            playState = PlayState.PLAY;
            if (videoClip) videoPlayer.Play();
            else VoiceOverPlayer.Play();
            exitPortalGenerator.ResumeSpawnCountdown();
        }

        public void StopAndReset()
        {
            if (videoClip) videoPlayer.Stop();
            else VoiceOverPlayer.Stop();
            if (subtitlePlayer != null) subtitlePlayer.StopSubtitle();
            playState = PlayState.STOP;
            timePlayed = 0;
            pausesScheduled.Clear();
            totalDuration = 0;
        }
    }

}
