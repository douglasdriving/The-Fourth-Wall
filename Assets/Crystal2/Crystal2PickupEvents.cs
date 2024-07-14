using System.Collections.Generic;
using UnityEngine;

public class Crystal2PickupEvents : MonoBehaviour
{
    [SerializeField] GameObject portalNarrationTriggerBox;
    [SerializeField] const int numberOfPiecesBackToStartNarrationAt = 8;

    public void StartPickupEvents()
    {
        List<GameObject> levelPiecesSpawned = FindObjectOfType<LevelGenerator>().levelPiecesSpawned;
        int indexOfLastPiece = levelPiecesSpawned.Count - 1;
        int indexOfPieceToStartNarrationAt = indexOfLastPiece - numberOfPiecesBackToStartNarrationAt;
        GameObject pieceToStartNarrationAt = levelPiecesSpawned[indexOfPieceToStartNarrationAt];
        Vector3 pos = pieceToStartNarrationAt.transform.position;
        Quaternion rot = pieceToStartNarrationAt.transform.rotation;
        portalNarrationTriggerBox.transform.position = pos;
        portalNarrationTriggerBox.transform.rotation = rot;
        portalNarrationTriggerBox.SetActive(true);
        enabled = false;
    }
}
