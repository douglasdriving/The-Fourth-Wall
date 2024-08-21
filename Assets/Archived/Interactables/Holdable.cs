using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Holdable : MonoBehaviour
{
  public void Pickup()
  {
    FindObjectOfType<ItemHolder>().PickupItem(transform);
  }
}