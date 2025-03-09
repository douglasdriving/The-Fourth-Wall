using LevelGeneration;
using UnityEngine;

public class ObjectScannerRay : MonoBehaviour
{
    private static ObjectScannerRay instance;
    [SerializeField] private LayerMask raycastMask = ~0; // Default to everything
    [SerializeField] private float sphereRadius = 0.01f; // Small radius to detect when inside colliders

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        CheckRay();
    }

    private void CheckRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray.origin, sphereRadius, ray.direction, Mathf.Infinity, raycastMask);
        
        // First try to find a hit that's in front of us (positive distance)
        foreach (RaycastHit hit in hits)
        {
            if (hit.distance > 0 && TryUnfreezeHitObject(hit.collider))
            {
                return;
            }
        }

        // If no hits in front, check for any collider we might be inside of
        Collider[] nearbyColliders = Physics.OverlapSphere(ray.origin, sphereRadius, raycastMask);
        foreach (Collider collider in nearbyColliders)
        {
            if (TryUnfreezeHitObject(collider))
            {
                return;
            }
        }
    }

    public static void ForceCheck()
    {
        if (instance != null)
        {
            instance.CheckRay();
        }
    }

    private static bool TryUnfreezeHitObject(Collider collider)
    {
        LevelPiece.Freezer unfreezer = collider.GetComponent<LevelPiece.Freezer>();
        if (unfreezer != null)
        {
            unfreezer.Unfreeze();
            return true;
        }
        return false;
    }
}
