using TMPro;
using UnityEngine;

public class WordPickupCounter : MonoBehaviour
{
  [SerializeField] TMP_Text wordCounter;
  int wordsCollected = 0;
  public void AddWord()
  {
    wordsCollected++;
    wordCounter.text = "Words: " + wordsCollected;
  }
}