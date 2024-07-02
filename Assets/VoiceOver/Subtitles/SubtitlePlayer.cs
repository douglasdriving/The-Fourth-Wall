using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitlePlayer : MonoBehaviour
{

    [SerializeField] TMP_Text subtitleText;

    const float lingerTime = 1f;
    static SubtitleData currentSubtitles;
    static int currentWordIndex = 0;
    static int currentSentanceIndex = 0;
    private float timeCurrentSubtitlesHasBeenPlayed = 0f;
    static float timeForNextSubtitleStep;
    static bool isLingering = false;

    private void Awake()
    {
        subtitleText.text = "";
    }

    public static void StartSubtitles(SubtitleData subtitles)
    {
        currentSubtitles = subtitles;
        currentWordIndex = -1;
        currentSentanceIndex = 0;
        timeForNextSubtitleStep = currentSubtitles.sentences[0].words[0].time;
        isLingering = false;
    }


    void Update()
    {
        if (currentSubtitles != null)
        {
            UpdateSubtitle();
        }
    }

    private void UpdateSubtitle()
    {
        timeCurrentSubtitlesHasBeenPlayed += Time.deltaTime;
        bool timeForNextSubtitleStep = timeCurrentSubtitlesHasBeenPlayed >= SubtitlePlayer.timeForNextSubtitleStep;
        if (timeForNextSubtitleStep)
        {
            TakeNextSubtitleStep();
        }
    }

    private void TakeNextSubtitleStep()
    {
        int wordsInCurrentSentence = currentSubtitles.sentences[currentSentanceIndex].words.Count;
        bool isLastWordOfSentence = currentWordIndex >= wordsInCurrentSentence - 1;

        if (isLingering)
        {
            StopSubtitle();
        }
        else if (isLastWordOfSentence)
        {
            MoveToNextSentance();
        }
        else
        {
            MoveToNextWordInSentance();
        }
    }

    private void MoveToNextWordInSentance()
    {
        currentWordIndex++;
        SentenceData currentSentence = currentSubtitles.sentences[currentSentanceIndex];
        WordTimestamp word = currentSentence.words[currentWordIndex];
        AddWordToUI(word.word);
        UpdateNextWordTime();
    }

    private static void UpdateNextWordTime()
    {
        SentenceData currentSentence = currentSubtitles.sentences[currentSentanceIndex];
        bool isLastWordOfSentence = currentWordIndex >= currentSentence.words.Count - 1;
        bool isLastSentence = currentSentanceIndex >= currentSubtitles.sentences.Count - 1;
        if (isLastSentence && isLastWordOfSentence)
        {
            timeForNextSubtitleStep += lingerTime;
            isLingering = true;
        }
        else if (isLastWordOfSentence)
        {
            SentenceData nextSentence = currentSubtitles.sentences[currentSentanceIndex + 1];
            WordTimestamp nextWord = nextSentence.words[0];
            timeForNextSubtitleStep = nextWord.time;
        }
        else
        {
            WordTimestamp nextWord = currentSentence.words[currentWordIndex + 1];
            timeForNextSubtitleStep = nextWord.time;
        }
    }

    private void MoveToNextSentance()
    {
        currentSentanceIndex++;
        currentWordIndex = 0;
        SentenceData sentence = currentSubtitles.sentences[currentSentanceIndex];
        string firstWordInSentence = sentence.words[0].word;
        StartNewSentenceInUI(firstWordInSentence);
        UpdateNextWordTime();
    }

    private void StopSubtitle()
    {
        currentSubtitles = null;
        currentSentanceIndex = 0;
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
