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

        if (GameRules.dangerousCharsOn)
        {
            string word = wordGO.GetComponentInChildren<TMP_Text>().text;
            word = word.ToUpper();
            char firstChar = word[0];
            if (firstChar == GameRules.dangerousChar)
            {
                throw new NotImplementedException();
                //we stepped on a dangerous character!
                //player dies
                //what happens when a player dies?
                //just reset to last platform?
                //or before the audio?
                //make sense that it would be before audio
                //but then we have to create a whole extra system.
                //maybe we need that.
                //should call the save point system and restore
            }
        }
        else
        {
            FindObjectOfType<WordPickupCounter>().AddWord();
        }

        Destroy(wordGO);
    }
}
