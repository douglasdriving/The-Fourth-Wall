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
        LevelPieceFreezer unfreezer = hit.collider.GetComponent<LevelPieceFreezer>();
        if (unfreezer != null)
        {
            unfreezer.Unfreeze();
        }
    }
}
