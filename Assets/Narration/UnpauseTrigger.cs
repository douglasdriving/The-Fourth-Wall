using System.Collections.Generic;
using UnityEngine;

namespace Narration
{
    public class UnpauseTrigger : MonoBehaviour
    {
        public bool isActive = false;
        List<UnpauseTrigger> linkedTriggers = new();

        public void SetActiveAndLink(List<UnpauseTrigger> triggersToLink)
        {
            isActive = true;
            linkedTriggers = triggersToLink;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isActive) return;
            if (!other.CompareTag("Player")) return;
            NarrationManager.Unpause();
            DeactivateSelfAndLinkedUnpauseTriggers();
        }

        void DeactivateSelfAndLinkedUnpauseTriggers()
        {
            foreach (UnpauseTrigger linkedTrigger in linkedTriggers)
            {
                linkedTrigger.DeactivateAndUnlink();
            }
            DeactivateAndUnlink();
        }

        public void DeactivateAndUnlink()
        {
            isActive = false;
            linkedTriggers.Clear();
        }
    }
}

