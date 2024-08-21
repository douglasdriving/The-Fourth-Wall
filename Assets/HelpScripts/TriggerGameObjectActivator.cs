using UnityEngine;

public class TriggerGameObjectActivator : MonoBehaviour
{
    [SerializeField] GameObject objectToActivate;
    [SerializeField] string objectTagToTriggerFor = "Player";

    void OnTriggerEnter(Collider other)
    {
        if (objectToActivate.activeInHierarchy)
        {
            return;
        }

        if (!other.CompareTag(objectTagToTriggerFor))
        {
            return;
        }

        objectToActivate.SetActive(true);
    }
}
