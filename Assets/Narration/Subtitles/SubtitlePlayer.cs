using System.Linq;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class SubtitlePlayer : MonoBehaviour
    {
        [SerializeField] TMP_Text subtitleText;
        // [SerializeField] bool singleWordSubtitles = true;
        [SerializeField] WordMover wordMover;
        [SerializeField] LevelGenerator levelGenerator;
        [SerializeField] GameObject wordPrefab;
        const float lingerTime = 1f;
        static SubtitleJsonData currentSubtitles;
        static int currentWordIndex = 0;
        static int currentSegmentIndex = 0;
        private float timeCurrentSubtitlesHasBeenPlayed = 0f;
        static float timeForNextSubtitleStep;
        static bool isLingering = false;

        private bool showNewWordsOnExistingLevelPieces = false;
        private bool isMovingBackwards = false;
        private int nextLevelPieceIndexToShowWordOn = 0;

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

            ShowWordInWorld(word);
        }

        public void StartShowingWordsOnExistingLevelPieces(int levelPieceIndexToStartFrom, bool moveBackwards)
        {
            showNewWordsOnExistingLevelPieces = true;
            isMovingBackwards = moveBackwards;
            nextLevelPieceIndexToShowWordOn = levelPieceIndexToStartFrom;
        }

        private void ShowWordInWorld(string word)
        {
            if (showNewWordsOnExistingLevelPieces)
            {
                ShowWordOnExistingLevelPiece(word);
            }
            else
            {
                levelGenerator.SpawnNextPiece(word, GetWordsLeftInSubtitle());
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
                Destroy(child);
            }
            //spawn a word canvas on the anchor
            GameObject wordGO = Instantiate(wordPrefab, wordAnchor);
            //set the word on the canvas to the word.
            wordGO.transform.GetComponentInChildren<TMP_Text>().text = word;
            //if we are moving backwards, rotate the word 180 deg on y
            if (isMovingBackwards)
            {
                wordGO.transform.Rotate(wordGO.transform.up * 180);
                nextLevelPieceIndexToShowWordOn--;
            }
            else
            {
                nextLevelPieceIndexToShowWordOn++;
            }
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

        // void AddWordToUI(string word)
        // {
        //     subtitleText.text += " " + word;
        // }

        // void StartNewSentenceInUI(string firstWord)
        // {
        //     subtitleText.text = firstWord;
        // }
    }

}
