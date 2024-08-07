using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordPickup : MonoBehaviour
{
    [SerializeField] GameObject wordGO;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CurrentGameRules.rules.dangerousCharsOn)
        {
            string word = wordGO.GetComponentInChildren<TMP_Text>().text;
            word = word.Trim();
            word = word.ToUpper();
            char firstChar = word[0];
            if (firstChar == CurrentGameRules.rules.dangerousChar)
            {
                Debug.Log("you died because you walked over a word with the first letter: " + firstChar + ". the word was: " + word);
                RespawnSystem.KillPlayerAndReset();
                return;
            }
        }

        FindObjectOfType<WordPickupCounter>().AddWord();
        Destroy(wordGO);
    }
}
