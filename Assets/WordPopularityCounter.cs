using System.Collections.Generic;
using UnityEngine;
using Narration;
using System;
using System.Linq;

public class WordPopularityCounter : MonoBehaviour
{
    [SerializeField] TextAsset[] scriptFiles;
    public static Dictionary<string, int> wordCount = new Dictionary<string, int>();
    private static Dictionary<string, float> normalizedWordPopularity = new Dictionary<string, float>();
    public static Tuple<string, int> mostPopularWord;

    void Awake()
    {
        RecountWordsInScripts();
        CalculateNormalizedPopularity();
    }

    void RecountWordsInScripts()
    {
        ClearWordCount();
        foreach (TextAsset scriptFile in scriptFiles)
        {
            SubtitleJsonData subtitleJsonData = SubtitleJsonReader.ReadSubtitleJson(scriptFile.text);
            foreach (SubtitleSegment segment in subtitleJsonData.segments)
            {
                foreach (SubtitleWord word in segment.words)
                {
                    string cleanWord = CleanWord(word.word);
                    int popularity = wordCount.ContainsKey(cleanWord) ? wordCount[cleanWord] + 1 : 1;
                    wordCount[cleanWord] = popularity;
                    if (mostPopularWord == null || popularity > mostPopularWord.Item2)
                    {
                        mostPopularWord = new Tuple<string, int>(cleanWord, popularity);
                    }
                }
            }
        }
        Debug.Log("updated word count");
    }

    private static void ClearWordCount()
    {
        wordCount.Clear();
        mostPopularWord = null;
        normalizedWordPopularity.Clear();
    }

    private static string CleanWord(string word)
    {
        string cleanWord = word.ToLower();
        cleanWord = cleanWord.Trim();
        cleanWord = cleanWord.Replace(".", "").Replace(",", "");
        cleanWord = cleanWord.Replace("!", "").Replace("?", "").Replace(";", "").Replace(":", "");
        return cleanWord;
    }

    private void CalculateNormalizedPopularity()
    {
        // Sort the words by popularity
        var sortedWords = wordCount.OrderBy(kvp => kvp.Value).ToList();
        int totalWords = sortedWords.Count;

        // Create the normalized popularity dictionary
        for (int i = 0; i < totalWords; i++)
        {
            string word = sortedWords[i].Key;
            float normalizedPosition = (float)i / (totalWords - 1);
            normalizedWordPopularity[word] = normalizedPosition;
        }
    }

    public static int GetPopularityForWord(string word)
    {
        string cleanWord = CleanWord(word);
        if (wordCount.ContainsKey(cleanWord))
        {
            return wordCount[cleanWord];
        }
        return 0;
    }

    public static float GetPopularityScaled(string word)
    {
        string cleanWord = CleanWord(word);
        if (wordCount.ContainsKey(cleanWord))
        {
            return (float)wordCount[cleanWord] / mostPopularWord.Item2;
        }
        return 0;
    }

    public static float GetPopularityForWordNormalized(string word)
    {
        string cleanWord = CleanWord(word);
        if (normalizedWordPopularity.ContainsKey(cleanWord))
        {
            return normalizedWordPopularity[cleanWord];
        }
        return 0;
    }
}
