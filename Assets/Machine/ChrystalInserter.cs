using UnityEngine;

/// <summary>
/// place on sockets to allow the player to insert items.
/// </summary>

public class ChrystalInserter : MonoBehaviour
{
    [SerializeField] GameObject socketedChrystal;
    ItemHolder itemHolder;
    InteractionScanner interactionScanner;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        itemHolder = player.GetComponent<ItemHolder>();
        interactionScanner = player.GetComponent<InteractionScanner>();
    }

    public void TryInsertChrystal() //should be called by the INTERACTABLE script, not by the player controls
    {
        bool playerIsHoldingACrystal = itemHolder.IsHoldingItem && itemHolder.ItemHeld.GetComponent<Interactable>().name == "Crystal";
        bool socketIsHighlighted = interactionScanner.highlightedInteractable == gameObject; //should not need this, should check in interactable script
        if (playerIsHoldingACrystal && socketIsHighlighted)
        {
            InsertChrystal();
        }
    }

    void InsertChrystal()
    {
        itemHolder.DestroyHeldItem();
        socketedChrystal.SetActive(true);
    }
}
