using UnityEngine;

public class ObjectHold : MonoBehaviour //change name to ItemHold (object is too confusing)
{

    //if we press on a specific key, pick up the object
    //hold object in front of us

    [SerializeField] Transform playerCamera;
    [SerializeField] float maxPickupDistance = 3f;
    [SerializeField] float highlightOutlineWidth;
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
}
