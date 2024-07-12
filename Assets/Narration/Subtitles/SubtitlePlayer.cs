using System.Linq;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class SubtitlePlayer : MonoBehaviour
    {
        [SerializeField] TMP_Text subtitleText;
        [SerializeField] bool singleWordSubtitles = true;
        [SerializeField] WordMover wordMover;
        [SerializeField] LevelGenerator levelGenerator;
        const float lingerTime = 1f;
        static SubtitleJsonData currentSubtitles;
        static int currentWordIndex = 0;
        static int currentSegmentIndex = 0;
        private float timeCurrentSubtitlesHasBeenPlayed = 0f;
        static float timeForNextSubtitleStep;
        static bool isLingering = false;

        private void Awake()
        {
            subtitleText.text = "";
        }

        public static void StartSubtitles(SubtitleJsonData subtitles)
        {
            currentSubtitles = subtitles;
            currentWordIndex = -1;
            currentSegmentIndex = 0;
            timeForNextSubtitleStep = currentSubtitles.segments[0].words[0].start;
            isLingering = false;
        }


        void Update()
        {
            if (currentSubtitles == null) return;

            timeCurrentSubtitlesHasBeenPlayed += Time.deltaTime;
            bool timeForNextSubtitleStep = timeCurrentSubtitlesHasBeenPlayed >= SubtitlePlayer.timeForNextSubtitleStep;

            if (!timeForNextSubtitleStep) return;

            TakeNextSubtitleStep();
        }

        private void TakeNextSubtitleStep()
        {
            int wordsInCurrentSentence = currentSubtitles.segments[currentSegmentIndex].words.Length;
            bool isLastWordOfSentence = currentWordIndex >= wordsInCurrentSentence - 1;

            if (isLingering)
            {
                StopSubtitle();
            }
            else if (isLastWordOfSentence)
            {
                MoveToNextSegment();
            }
            else
            {
                MoveToNextWordInSentance();
            }
        }

        private void MoveToNextWordInSentance()
        {
            currentWordIndex++;
            SubtitleSegment currentSegment = currentSubtitles.segments[currentSegmentIndex];
            string word = currentSegment.words[currentWordIndex].word;
            // if (singleWordSubtitles)
            // {
            //     subtitleText.text = word.word;
            // }
            // else
            // {
            //     AddWordToUI(word.word);
            // }
            UpdateNextWordTime();
            // wordMover.UpdateWordPosition();

            levelGenerator.SpawnNextPiece(word, GetWordsLeftInSubtitle());
        }

        private static int GetWordsLeftInSubtitle()
        {
            SubtitleSegment currentSegment = currentSubtitles.segments[currentSegmentIndex];
            int wordsLeftInCurrentSegment = currentSegment.words.Length - (currentWordIndex + 1);
            int wordsLeftInUpcomingSegments = 0;
            for (int i = currentSegmentIndex + 1; i < currentSubtitles.segments.Length; i++)
            {
                wordsLeftInUpcomingSegments += currentSubtitles.segments[i].words.Length;
            }
            int wordsLeftInSubtitle = wordsLeftInCurrentSegment + wordsLeftInUpcomingSegments;
            return wordsLeftInSubtitle;
        }

        private static void UpdateNextWordTime()
        {
            SubtitleSegment currentSegment = currentSubtitles.segments[currentSegmentIndex];
            bool isLastWordOfSegment = currentWordIndex >= currentSegment.words.Length - 1;
            bool isLastSegment = currentSegmentIndex >= currentSubtitles.segments.Length - 1;
            if (isLastSegment && isLastWordOfSegment)
            {
                timeForNextSubtitleStep += lingerTime;
                isLingering = true;
            }
            else if (isLastWordOfSegment)
            {
                SubtitleSegment nextSegment = currentSubtitles.segments[currentSegmentIndex + 1];
                SubtitleWord nextWord = nextSegment.words[0];
                timeForNextSubtitleStep = nextWord.start;
            }
            else
            {
                SubtitleWord nextWord = currentSegment.words[currentWordIndex + 1];
                timeForNextSubtitleStep = nextWord.start;
            }
        }

        private void MoveToNextSegment()
        {
            currentSegmentIndex++;
            currentWordIndex = 0;
            SubtitleSegment segment = currentSubtitles.segments[currentSegmentIndex];
            string firstWordInSegment = segment.words[0].word;
            // StartNewSentenceInUI(firstWordInSegment);
            UpdateNextWordTime();
            // wordMover.UpdateWordPosition();
            levelGenerator.SpawnNextPiece(firstWordInSegment, GetWordsLeftInSubtitle());
        }

        private void StopSubtitle()
        {
            currentSubtitles = null;
            currentSegmentIndex = 0;
            currentWordIndex = 0;
            timeCurrentSubtitlesHasBeenPlayed = 0;
            timeForNextSubtitleStep = 0;
            isLingering = false;
            subtitleText.text = "";
        }

        void AddWordToUI(string word)
        {
            subtitleText.text += " " + word;
        }

        void StartNewSentenceInUI(string firstWord)
        {
            subtitleText.text = firstWord;
        }
    }

}
