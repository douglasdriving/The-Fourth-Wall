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

    public void PickupItem(Transform item)
    {
        item.parent = itemHoldPos;
        item.position = itemHoldPos.position;
        item.rotation = itemHoldPos.rotation;
        itemHeld = item;
    }

    public void DestroyHeldItem()
    {
        Destroy(itemHeld.gameObject);
        itemHeld = null;
    }
}
