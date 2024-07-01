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

    public void TryInsertChrystal()
    {
        bool playerIsHoldingACrystal = itemHolder.IsHoldingItem && itemHolder.ItemHeld.GetComponent<Interactable>().InteractableName == "Crystal";
        if (playerIsHoldingACrystal)
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
