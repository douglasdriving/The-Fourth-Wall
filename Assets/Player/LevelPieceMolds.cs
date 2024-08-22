using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// keeps track of all the level piece molds in front of the player and lets them be copied
/// </summary>
public class LevelPieceMolds : MonoBehaviour
{
    [SerializeField] List<GameObject> molds = new List<GameObject>();
    int indexOfLastMoldCopied = -1;

    public GameObject CopyMold()
    {
        int indexOfMoldToCopy = indexOfLastMoldCopied + 1;

        if (indexOfMoldToCopy >= molds.Count)
        {
            indexOfMoldToCopy = 0;
        }

        GameObject moldToCopy = molds[indexOfMoldToCopy];
        GameObject levelPieceCopy = Instantiate(moldToCopy);
        levelPieceCopy.SetActive(true);

        indexOfLastMoldCopied = indexOfMoldToCopy;

        return levelPieceCopy;
    }
}
