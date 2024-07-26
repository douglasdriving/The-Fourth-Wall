using System.Collections.Generic;
using Narration;
using UnityEngine;

public class Crystal2PickupEvents : MonoBehaviour
{
    [SerializeField] GameObject portalNarrationTriggerBox;
    [SerializeField] int numberOfPiecesBackToStartNarrationAt = 5;
    [SerializeField] int levelPiecesBackToSpawnFirstWordOn = 8;

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

        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        int indexOfLastLevelPiece = levelGenerator.levelPiecesSpawned.Count - 1;
        int indexOfPieceToStartSpawningWordsFrom = indexOfLastLevelPiece - levelPiecesBackToSpawnFirstWordOn;
        FindObjectOfType<SubtitlePlayer>().StartSpawningBackwards(indexOfPieceToStartSpawningWordsFrom);
    }
}
