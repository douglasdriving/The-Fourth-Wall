using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] InteractionScanner scanner;
    [SerializeField] Transform itemHoldPos;
    Transform itemHeld = null;
    public bool IsHoldingItem
    {
        get { return itemHeld != null; }
    }
    public Transform ItemHeld
    {
        get { return itemHeld; }
    }

    void OnInteract()
    {
        bool handIsFree = itemHeld == null;
        bool itemAvailableForPickup = scanner.highlightedInteractable != null && scanner.highlightedInteractable.CompareTag("Holdable");
        if (handIsFree && itemAvailableForPickup)
        {
            PickupItem(scanner.highlightedInteractable.transform);
        }
    }

    private void PickupItem(Transform item)
    {
        item.parent = itemHoldPos;
        item.position = itemHoldPos.position;
        item.rotation = itemHoldPos.rotation;
        itemHeld = item;
        scanner.ClearHighlight();
    }

    public void DestroyHeldItem()
    {
        Destroy(itemHeld.gameObject);
        itemHeld = null;
    }
}
