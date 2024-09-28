using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace LevelGeneration
{
  /// <summary>
  /// adds pieces to the level
  /// </summary>
  [RequireComponent(typeof(WalkwayGenerator))]
  [RequireComponent(typeof(AndRailSpawner))]
  public class LevelGenerator : MonoBehaviour
  {
    WalkwayGenerator walkwayGenerator;
    public List<GameObject> levelPiecesSpawned = new();
    AndRailSpawner andRailSpawner;
    [SerializeField] bool isSpawningAndRails = false;

    void Awake()
    {
      if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
      walkwayGenerator = GetComponent<WalkwayGenerator>();
      andRailSpawner = GetComponent<AndRailSpawner>();
    }

    public GameObject SpawnNextPiece(string word)
    {
      Vector3 lastPieceEndPointTargetPos = GetEndPointOfLastPieceSpawned();
      GameObject levelPiece;

      word = word.Trim();

      if (isSpawningAndRails && word.ToLower() == "and")
      {
        levelPiece = andRailSpawner.SpawnRail(lastPieceEndPointTargetPos);
      }
      else
      {
        levelPiece = walkwayGenerator.AddPieceToWalkway(lastPieceEndPointTargetPos, word, GetLastPieceWord());
      }

      levelPiecesSpawned.Add(levelPiece);
      return levelPiece;
    }

    private Vector3 GetEndPointOfLastPieceSpawned()
    {

      Transform lastPiece = levelPiecesSpawned.Last().transform;
      Transform lastPieceEndPoint = lastPiece.GetComponentInChildren<LevelPieceEndPoint>().transform;

      if (lastPieceEndPoint == null) throw new System.Exception("no end point found on last piece. make sure there is a child with the tag LevelPieceEnd");

      Vector3 lastPieceTargetPos = lastPiece.GetComponent<LevelPiecePositioner>().targetPos;
      float distanceBetweenLastPieceAndLastPieceEndPoint = Vector3.Distance(lastPiece.position, lastPieceEndPoint.position);
      Vector3 lastPieceEndPointTargetPos = lastPieceTargetPos + Vector3.forward * distanceBetweenLastPieceAndLastPieceEndPoint;

      return lastPieceEndPointTargetPos;
    }

    private string GetLastPieceWord()
    {
      TMP_Text lastPieceText = levelPiecesSpawned.Last().GetComponentInChildren<TMP_Text>();
      if (lastPieceText == null)
      {
        return "";
      }
      else
      {
        return lastPieceText.text;
      }
    }
  }
}
