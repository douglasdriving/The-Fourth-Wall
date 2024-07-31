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

        if (CurrentGameRules.currentGameRules.dangerousCharsOn)
        {
            string word = wordGO.GetComponentInChildren<TMP_Text>().text;
            word = word.Trim();
            word = word.ToUpper();
            char firstChar = word[0];
            if (firstChar == CurrentGameRules.currentGameRules.dangerousChar)
            {
                RespawnSystem.KillPlayerAndReset();
                return;
            }
        }

        FindObjectOfType<WordPickupCounter>().AddWord();
        Destroy(wordGO);
    }
}
