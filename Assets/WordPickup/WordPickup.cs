using UnityEngine;

public class WordPickup : MonoBehaviour
{
    [SerializeField] GameObject wordGO;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        FindObjectOfType<WordPickupCounter>().AddWord();
        Destroy(wordGO);
    }
}
