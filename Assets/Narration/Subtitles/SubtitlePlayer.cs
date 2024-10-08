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
        [SerializeField] GameObject wordPrefab;
        const float lingerTime = 1f;
        SubtitleJsonData currentSubtitles;
        int currentWordIndex = 0;
        int currentSegmentIndex = 0;
        float timeForNextSubtitleStep = 0;
        bool isLingering = false;

        public int nextLevelPieceIndexToShowWordOn = 0;
        SubtitleMode mode = SubtitleMode.SpawnWithNewLevelPiece;

        public void SetMode(SubtitleMode mode, int levelPieceIndexToStartSpawningFrom)
        {
            this.mode = mode;
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
            if (NarrationManager.playState != NarrationManager.PlayState.PLAY) return;

            bool isTimeForNextSubtitleStep = NarrationManager.timeCurrentNarrationHasPlayed >= timeForNextSubtitleStep;

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

        private void MoveToNextSegment()
        {
            currentSegmentIndex++;
            currentWordIndex = 0;
            SubtitleSegment segment = currentSubtitles.segments[currentSegmentIndex];
            SubtitleWord word = segment.words[0];
            UpdateNextWordTime();
            ShowWordInWorld(word.word);
        }


        private void MoveToNextWordInSentance()
        {
            currentWordIndex++;
            SubtitleSegment currentSegment = currentSubtitles.segments[currentSegmentIndex];
            SubtitleWord word = currentSegment.words[currentWordIndex];
            UpdateNextWordTime();
            ShowWordInWorld(word.word);
        }

        private void ShowWordInWorld(string word)
        {
            if (mode == SubtitleMode.SpawnBackwardOnLevel || mode == SubtitleMode.SpawnForwardOnLevel)
            {
                ShowWordOnExistingLevelPiece(word); ///!!!! IF ItS PAUSE; WE NEED TO SPAWN A UNPAUSE TRIGGER VOLUME
            }
            else if (mode == SubtitleMode.SpawnWithNewLevelPiece)
            {
                levelGenerator.SpawnNextPiece(word);
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

        public void StopSubtitle()
        {
            currentSubtitles = null;
            currentSegmentIndex = 0;
            currentWordIndex = 0;
            timeForNextSubtitleStep = 0;
            isLingering = false;
        }
    }

}
