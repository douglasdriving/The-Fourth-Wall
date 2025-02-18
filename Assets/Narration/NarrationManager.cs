using System.Collections;
using System.Collections.Generic;
using LevelGeneration;
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

        const int numberOfUnpausePiecesOnPause = 8;
        const float pauseDelay = 0.12f;

        static SubtitlePlayer subtitlePlayer;
        public static PlayState playState = PlayState.STOP;
        public static float timePlayed = 0;
        static float totalDuration = 0;
        static List<float> pausesScheduled = new();

        [SerializeField] AudioClip audioClip;
        [SerializeField] VideoClip videoClip;
        [SerializeField] TextAsset subtitle;
        [SerializeField] float startDelay = 1.5f;
        [SerializeField] bool endSceneOnEnd = false;
        [SerializeField] string placeHolderText = "This is the placeholder devlog text. It should be replaced with a proper description of the class.";
        [SerializeField] VideoPlayer videoPlayer;

        void Awake()
        {
            subtitlePlayer = FindObjectOfType<SubtitlePlayer>();
            if (videoClip != null && audioClip != null)
            {
                Debug.LogWarning("both audio and video provided. will only play video.");
            }
        }

        void Start()
        {
            Invoke("PlayNarration", startDelay);
        }

        void PlayNarration()
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
            SetPausesFromSubtitle(subtitleData);
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

        private static void SetPausesFromSubtitle(SubtitleJsonData subtitleData)
        {
            pausesScheduled.Clear();
            foreach (SubtitleWord word in subtitleData.GetWords())
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

        private void Pause()
        {
            playState = PlayState.PAUSE;
            VoiceOverPlayer.Pause();
            videoPlayer.Pause();
            UnpauseTriggerActivator.ActivateUnpauseTriggerOnLastPieces(numberOfUnpausePiecesOnPause);
        }

        public void Resume()
        {
            playState = PlayState.PLAY;
            if (videoClip) videoPlayer.Play();
            else VoiceOverPlayer.Play();
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
