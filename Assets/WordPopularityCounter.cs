using System.Collections.Generic;
using UnityEngine;
using Narration;
using System;

public class WordPopularityCounter : MonoBehaviour
{
    [SerializeField] TextAsset[] scriptFiles;
    public static Dictionary<string, int> wordPopularity = new Dictionary<string, int>();
    public static Tuple<string, int> mostPopularWord;

    void Awake()
    {
        CountWordsInScripts();
    }

    void CountWordsInScripts()
    {
        foreach (TextAsset scriptFile in scriptFiles)
        {
            SubtitleJsonData subtitleJsonData = SubtitleJsonReader.ReadSubtitleJson(scriptFile.text);
            foreach (SubtitleSegment segment in subtitleJsonData.segments)
            {
                foreach (SubtitleWord word in segment.words)
                {
                    string cleanWord = CleanWord(word.word);
                    int popularity = wordPopularity.ContainsKey(cleanWord) ? wordPopularity[cleanWord] + 1 : 1;
                    wordPopularity[cleanWord] = popularity;
                    if (mostPopularWord == null || popularity > mostPopularWord.Item2)
                    {
                        mostPopularWord = new Tuple<string, int>(cleanWord, popularity);
                    }
                }
            }
        }
    }

    private static string CleanWord(string word)
    {
        string cleanWord = word.ToLower();
        cleanWord = cleanWord.Trim();
        cleanWord = cleanWord.Replace(".", "").Replace(",", "");
        cleanWord = cleanWord.Replace("!", "").Replace("?", "").Replace(";", "").Replace(":", "");
        return cleanWord;
    }

    public static int GetPopularityForWord(string word)
    {
        string cleanWord = CleanWord(word);
        if (wordPopularity.ContainsKey(cleanWord))
        {
            return wordPopularity[cleanWord];
        }
        return 0;
    }
}
