using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPickup : MonoBehaviour
{
    [SerializeField] GameObject word;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        FindObjectOfType<WordPickupCounter>().AddWord();
        Destroy(word);
    }
}
