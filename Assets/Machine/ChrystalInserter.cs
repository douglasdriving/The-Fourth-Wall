using UnityEngine;

/// <summary>
/// place on sockets to allow the player to insert items.
/// </summary>

[RequireComponent(typeof(Interactable))]
public class ChrystalInserter : MonoBehaviour
{
    [SerializeField] Machine machine;
    [SerializeField] GameObject socketedChrystal;
    ItemHolder itemHolder;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        itemHolder = player.GetComponent<ItemHolder>();
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
        GetComponent<Interactable>().RemoveInteractability();
        machine.AddChrystal();
    }
}
