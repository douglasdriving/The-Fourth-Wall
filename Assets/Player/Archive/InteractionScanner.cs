using UnityEngine;

public class InteractionScanner : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float maxInteractionDistance = 4f;
    public Interactable highlightedInteractable = null;

    private void Update()
    {
        ScanForItem();
    }

    void OnInteract()
    {
        if (highlightedInteractable != null)
        {
            highlightedInteractable.Interact();
        }
    }

    void OnInteractSecondary()
    {
        if (highlightedInteractable != null)
        {
            highlightedInteractable.InteractSecondary();
        }
    }

    private void ScanForItem()
    {
        RaycastHit hit;
        bool somethingIsInFrontOfPlayer = Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, maxInteractionDistance);
        if (somethingIsInFrontOfPlayer)
        {
            GameObject detectedObject = hit.collider.gameObject;
            Interactable interactable = detectedObject.GetComponent<Interactable>();
            if (interactable != null && interactable.IsInteractable)
            {
                bool isLookingAtNewInteractable = highlightedInteractable != interactable;
                if (isLookingAtNewInteractable)
                {
                    SwitchHighlightedItem(interactable);
                }
            }
            else
            {
                ClearHighlight();
            }
        }
        else
        {
            ClearHighlight();
        }
    }

    private void SwitchHighlightedItem(Interactable newInteractable)
    {
        ClearHighlight();
        highlightedInteractable = newInteractable;
        highlightedInteractable.Highlight();
    }

    public void ClearHighlight()
    {
        if (highlightedInteractable != null)
        {
            highlightedInteractable.RemoveHighlight();
            highlightedInteractable = null;
        }
    }
}
