using TMPro;
using UnityEngine;

/// <summary>
/// spawns words as blocks that stops the player
/// </summary>
public class BlockWordSpawner : MonoBehaviour
{
    [SerializeField] GameObject letterBlockPrefab;
    float letterBlockWidth = 1;

    void Awake()
    {
        letterBlockWidth = letterBlockPrefab.transform.localScale.x;
    }

    public GameObject SpawnWordBlock(string word, Vector3 wordPos, Vector3 wordForward)
    {
        GameObject wordBlock = new GameObject("word block");
        wordBlock.transform.position = Vector3.zero;

        int blocksSpawned = 0;
        int letterCount = word.Length;
        float wordBlockWidth = letterBlockWidth * letterCount;
        float halfWordBlockWidth = wordBlockWidth / 2;
        float halfLetterBlockWidth = letterBlockWidth / 2;
        float distanceFromWordCenterToFirstLetterCenter = halfWordBlockWidth - halfLetterBlockWidth;
        Vector3 firstLetterPos = Vector3.left * distanceFromWordCenterToFirstLetterCenter;

        foreach (char c in word)
        {
            GameObject letterBlock = Instantiate(letterBlockPrefab);
            letterBlock.GetComponent<LetterBlock>().SetLetter(c);
            letterBlock.transform.position = firstLetterPos + (Vector3.right * letterBlockWidth * blocksSpawned);
            letterBlock.transform.parent = wordBlock.transform;
            blocksSpawned++;
        }

        wordBlock.transform.position = wordPos;
        wordBlock.transform.forward = wordForward;

        return wordBlock;
    }
}
