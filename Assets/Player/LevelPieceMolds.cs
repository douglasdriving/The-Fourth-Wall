using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// keeps track of all the level piece molds in front of the player and lets them be copied
    /// </summary>
    public class LevelPieceMolds : MonoBehaviour
    {
        [SerializeField] List<GameObject> molds = new List<GameObject>();
        int indexOfLastMoldCopied = -1;

        public GameObject CopyNextMold()
        {
            int indexOfMoldToCopy = indexOfLastMoldCopied + 1;

            if (indexOfMoldToCopy >= molds.Count)
            {
                indexOfMoldToCopy = 0;
            }

            GameObject moldToCopy = molds[indexOfMoldToCopy];
            GameObject levelPieceCopy = Instantiate(moldToCopy);

            levelPieceCopy.transform.position = moldToCopy.transform.position;
            levelPieceCopy.transform.rotation = moldToCopy.transform.rotation;
            levelPieceCopy.transform.localScale = moldToCopy.transform.localScale;

            levelPieceCopy.SetActive(true);

            indexOfLastMoldCopied = indexOfMoldToCopy;

            return levelPieceCopy;
        }
    }
}

