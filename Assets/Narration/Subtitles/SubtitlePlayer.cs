using System;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class SubtitlePlayer : MonoBehaviour
    {
        [SerializeField] TMP_Text subtitleText;
        [SerializeField] WordMover wordMover;
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] GameObject wordPrefab;
        const float lingerTime = 1f;
        SubtitleJsonData currentSubtitles;
        int currentWordIndex = 0;
        int currentSegmentIndex = 0;
        private float timeCurrentSubtitlesHasBeenPlayed = 0f;
        float timeForNextSubtitleStep = 0;
        bool isLingering = false;

        public int nextLevelPieceIndexToShowWordOn = 0;
        SubtitleMode mode = SubtitleMode.SpawnWithNewLevelPiece;

        private void Awake()
        {
            subtitleText.text = "";
        }


        public void StartSpawningBackwards(int levelPieceIndexToStartSpawningFrom)
        {
            mode = SubtitleMode.SpawnBackwardOnLevel;
            nextLevelPieceIndexToShowWordOn = levelPieceIndexToStartSpawningFrom;
        }

        public void StartSubtitles(SubtitleJsonData subtitles)
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
            bool isTimeForNextSubtitleStep = timeCurrentSubtitlesHasBeenPlayed >= timeForNextSubtitleStep;

            if (!isTimeForNextSubtitleStep) return;

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
            UpdateNextWordTime();
            ShowWordInWorld(word);
        }

        private void ShowWordInWorld(string word)
        {
            if (mode == SubtitleMode.SpawnBackwardOnLevel || mode == SubtitleMode.SpawnForwardOnLevel)
            {
                ShowWordOnExistingLevelPiece(word);
            }
            else if (mode == SubtitleMode.SpawnWithNewLevelPiece)
            {
                levelGenerator.SpawnNextPiece(word, GetWordsLeftInSubtitle());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void ShowWordOnExistingLevelPiece(string word)
        {
            //user the level piece index to find the piece to show the word on
            GameObject levelPiece = levelGenerator.levelPiecesSpawned[nextLevelPieceIndexToShowWordOn];
            //get the word canvas anchor in it
            Transform wordAnchor = null;
            foreach (Transform child in levelPiece.transform)
            {
                if (child.CompareTag("WordAnchor"))
                {
                    wordAnchor = child;
                    break;
                }
            }
            if (!wordAnchor) Debug.LogError("Cant show word on existing level piece. No word anchor found as a child of the piece");
            //clear all children of the anchor
            foreach (Transform child in wordAnchor)
            {
                Destroy(child.gameObject);
            }
            //spawn a word canvas on the anchor
            GameObject wordGO = Instantiate(wordPrefab, wordAnchor);
            //set the word on the canvas to the word.
            wordGO.transform.GetComponentInChildren<TMP_Text>().text = word;
            //if we are moving backwards, rotate the word 180 deg on y
            if (mode == SubtitleMode.SpawnBackwardOnLevel)
            {
                wordGO.transform.Rotate(wordGO.transform.up * 180);
                nextLevelPieceIndexToShowWordOn--;
            }
            else
            {
                nextLevelPieceIndexToShowWordOn++;
            }
        }

        private int GetWordsLeftInSubtitle()
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

        private void UpdateNextWordTime()
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
            UpdateNextWordTime();
            ShowWordInWorld(firstWordInSegment);
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
    }

}
