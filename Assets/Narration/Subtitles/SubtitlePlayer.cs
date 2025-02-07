using System;
using LevelGeneration;
using TMPro;
using UnityEngine;

namespace Narration
{
    /// <summary>
    /// Plays subtitles in the world
    /// </summary>
    public class SubtitlePlayer : MonoBehaviour
    {
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] TextMeshProUGUI wordText;
        const float lingerTime = 1f;
        SubtitleJsonData currentSubtitle;
        int currentWordIndex = 0;
        int currentSegmentIndex = 0;
        float timeForNextSubtitleStep = 0;
        bool isLingering = false;

        void Awake()
        {
            if (levelGenerator) wordText.gameObject.SetActive(false);
            else wordText.gameObject.SetActive(true);
        }

        public void StartSubtitles(SubtitleJsonData subtitle)
        {
            currentSubtitle = subtitle;
            currentWordIndex = -1;
            currentSegmentIndex = 0;
            timeForNextSubtitleStep = currentSubtitle.segments[0].words[0].start;
            isLingering = false;
        }

        void Update()
        {
            if (currentSubtitle == null) return;
            if (NarrationManager.playState != NarrationManager.PlayState.PLAY) return;
            bool isTimeForNextSubtitleStep = NarrationManager.timePlayed >= timeForNextSubtitleStep;
            if (!isTimeForNextSubtitleStep) return;
            TakeNextSubtitleStep();
        }

        private void TakeNextSubtitleStep()
        {
            int wordsInCurrentSegment = currentSubtitle.segments[currentSegmentIndex].words.Length;
            bool isLastWordOfSegment = currentWordIndex >= wordsInCurrentSegment - 1;

            if (isLingering)
            {
                StopSubtitle();
            }
            else if (isLastWordOfSegment)
            {
                MoveToNextSegment();
            }
            else
            {
                MoveToNextWordInSentance();
            }
        }

        private void MoveToNextSegment()
        {
            currentSegmentIndex++;
            currentWordIndex = 0;
            SubtitleSegment segment = currentSubtitle.segments[currentSegmentIndex];
            SubtitleWord word = segment.words[0];
            UpdateNextWordTime();
            ShowWordInWorld(word.word, true);
        }


        private void MoveToNextWordInSentance()
        {
            currentWordIndex++;
            SubtitleSegment currentSegment = currentSubtitle.segments[currentSegmentIndex];
            SubtitleWord word = currentSegment.words[currentWordIndex];
            UpdateNextWordTime();
            ShowWordInWorld(word.word);
        }

        private void ShowWordInWorld(string word, bool isEndOFSentence = false)
        {
            if (levelGenerator) levelGenerator.SpawnNextPiece(word, isEndOFSentence);
            else wordText.text = word;
        }

        private void UpdateNextWordTime()
        {
            SubtitleSegment currentSegment = currentSubtitle.segments[currentSegmentIndex];
            bool isLastWordOfSegment = currentWordIndex >= currentSegment.words.Length - 1;
            bool isLastSegment = currentSegmentIndex >= currentSubtitle.segments.Length - 1;
            if (isLastSegment && isLastWordOfSegment)
            {
                timeForNextSubtitleStep += lingerTime;
                isLingering = true;
            }
            else if (isLastWordOfSegment)
            {
                SubtitleSegment nextSegment = currentSubtitle.segments[currentSegmentIndex + 1];
                SubtitleWord nextWord = nextSegment.words[0];
                timeForNextSubtitleStep = nextWord.start;
            }
            else
            {
                SubtitleWord nextWord = currentSegment.words[currentWordIndex + 1];
                timeForNextSubtitleStep = nextWord.start;
            }
        }

        public void StopSubtitle()
        {
            currentSubtitle = null;
            currentSegmentIndex = 0;
            currentWordIndex = 0;
            timeForNextSubtitleStep = 0;
            isLingering = false;
        }
    }

}
