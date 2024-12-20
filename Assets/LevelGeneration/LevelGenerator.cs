using System.Collections.Generic;
using System.Linq;
using LevelGeneration.ThePiece;
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
    ThePieceSpawner thePieceSpawner;
    [SerializeField] bool isSpawningAndRails = false;
    [SerializeField] bool isSpawningThePieces = false;

    void Awake()
    {
      if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
      walkwayGenerator = GetComponent<WalkwayGenerator>();
      andRailSpawner = GetComponent<AndRailSpawner>();
      thePieceSpawner = GetComponent<ThePieceSpawner>();
    }

    public GameObject SpawnNextPiece(string word)
    {
      GameObject levelPiece;
      word = word.Trim();
      Vector3 lastPieceFinalWalkoffPoint = GetLastPieceFinalWalkoffPoint();

      if (isSpawningAndRails && word.ToLower() == "and")
      {
        levelPiece = andRailSpawner.SpawnRail(lastPieceFinalWalkoffPoint);
      }
      else if (isSpawningThePieces && word.ToLower() == "the")
      {
        levelPiece = thePieceSpawner.Spawn(lastPieceFinalWalkoffPoint);
      }
      else
      {
        string lastPieceWord = GetLastPieceWord();
        levelPiece = walkwayGenerator.AddPieceToWalkway(lastPieceFinalWalkoffPoint, word, lastPieceWord);
      }

      levelPiece.GetComponent<LevelPiecePositioner>().FreezeInPlace(); // Peac5

      levelPiecesSpawned.Add(levelPiece);
      return levelPiece;
    }

    private Vector3 GetLastPieceFinalWalkoffPoint()
    {
      GameObject lastPiece = levelPiecesSpawned.Last();
      Vector3 lastPieceEndPoint = lastPiece.GetComponent<LevelPiecePositioner>().GetFinalWalkOffPoint();
      return lastPieceEndPoint;
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
