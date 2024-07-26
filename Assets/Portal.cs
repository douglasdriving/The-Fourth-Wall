using UnityEngine;

/// <summary>
/// transports the player to a given point when walking into a portal
/// </summary>
public class Portal : MonoBehaviour
{
    public Vector3 destination;
    public Vector3 forwardDirAfterTeleport;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.transform.position = destination;
        other.transform.forward = forwardDirAfterTeleport;
    }
}
