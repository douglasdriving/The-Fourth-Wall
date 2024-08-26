using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LevelGeneration
{
  /// <summary>
  /// adds pieces to the level
  /// </summary>
  [RequireComponent(typeof(WalkwayGenerator))]
  public class LevelGenerator : MonoBehaviour
  {
    WalkwayGenerator walkwayGenerator;
    public List<GameObject> levelPiecesSpawned = new();
    public delegate void LevelPieceSpawned(GameObject piece);
    public static event LevelPieceSpawned OnLevelPieceSpawned;

    void Awake()
    {
      if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
      walkwayGenerator = GetComponent<WalkwayGenerator>();
    }

    public GameObject SpawnNextPiece(string pieceWord)
    {
      Transform lastPiece = levelPiecesSpawned.Last().transform;
      GameObject piece = walkwayGenerator.AddPieceToWalkway(lastPiece, pieceWord);
      levelPiecesSpawned.Add(piece);
      OnLevelPieceSpawned?.Invoke(piece);
      return piece;
    }
  }
}
