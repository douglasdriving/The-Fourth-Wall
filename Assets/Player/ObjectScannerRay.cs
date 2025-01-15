using LevelGeneration;
using UnityEngine;

public class ObjectScannerRay : MonoBehaviour
{
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            TryUnfreezeHitObject(hit);
        }
    }

    private static void TryUnfreezeHitObject(RaycastHit hit)
    {
        LevelPiece.Freezer unfreezer = hit.collider.GetComponent<LevelPiece.Freezer>();
        if (unfreezer != null)
        {
            unfreezer.Unfreeze();
        }
    }
}
