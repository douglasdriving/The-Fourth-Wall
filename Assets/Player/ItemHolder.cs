using UnityEngine;

public class ItemHolder : MonoBehaviour //change name to ItemHold (object is too confusing)
{
    [SerializeField] Transform playerCamera;
    [SerializeField] float maxPickupDistance = 3f;
    [SerializeField] float highlightOutlineWidth;
    [SerializeField] Transform itemHoldPos;
    GameObject itemHeld = null;
    GameObject holdableItemInFrontOfPlayer = null;


    private void Update()
    {
        bool isHoldingItem = itemHeld != null;
        if (!isHoldingItem)
        {
            ScanForItem();
        }
    }

    void OnInteract()
    {
        bool handIsFree = itemHeld == null;
        bool itemAvailableForPickup = holdableItemInFrontOfPlayer != null;
        if (handIsFree && itemAvailableForPickup)
        {
            PickupItemInFrontOfPlayer();
        }
    }

    private void ScanForItem()
    {
        RaycastHit hit;
        bool somethingIsInFrontOfPlayer = Physics.Raycast(playerCamera.position, playerCamera.TransformDirection(Vector3.forward), out hit, maxPickupDistance);
        if (somethingIsInFrontOfPlayer)
        {
            Collider hitCollider = hit.collider;
            bool itemCanBeHeld = hitCollider.CompareTag("Holdable");
            if (itemCanBeHeld)
            {
                GameObject detectedItem = hitCollider.gameObject;
                bool isLookingAtNewItem = holdableItemInFrontOfPlayer != detectedItem;
                if (isLookingAtNewItem)
                {
                    SwitchItemInFrontOfPlayer(detectedItem);
                }
            }
            else
            {
                ClearnItemInFrontOfPlayerIfExists();
            }
        }
        else
        {
            ClearnItemInFrontOfPlayerIfExists();
        }
    }

    private void SwitchItemInFrontOfPlayer(GameObject newItemInFrontOfPlayer)
    {
        ClearnItemInFrontOfPlayerIfExists();
        holdableItemInFrontOfPlayer = newItemInFrontOfPlayer;
        Outline outlineOfNewItem = holdableItemInFrontOfPlayer.GetComponent<Outline>();
        outlineOfNewItem.OutlineMode = Outline.Mode.OutlineAll;
        outlineOfNewItem.OutlineWidth = highlightOutlineWidth;
    }

    private void ClearnItemInFrontOfPlayerIfExists()
    {
        bool wasLookingAtItem = holdableItemInFrontOfPlayer != null;
        if (wasLookingAtItem)
        {
            holdableItemInFrontOfPlayer.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
            holdableItemInFrontOfPlayer = null;
        }
    }

    private void PickupItemInFrontOfPlayer()
    {
        holdableItemInFrontOfPlayer.transform.parent = itemHoldPos;
        holdableItemInFrontOfPlayer.transform.position = itemHoldPos.position;
        holdableItemInFrontOfPlayer.transform.rotation = itemHoldPos.rotation;
        itemHeld = holdableItemInFrontOfPlayer;
        ClearnItemInFrontOfPlayerIfExists();
    }

}
