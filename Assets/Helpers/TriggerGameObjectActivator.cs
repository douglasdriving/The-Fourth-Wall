using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// Actives a game object on collision with a tagged game object
    /// </summary>
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

}
