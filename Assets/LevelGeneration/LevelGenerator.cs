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
    ThePieceSpawner thePieceSpawner;
    [SerializeField] bool isSpawningAndRails = false;
    [SerializeField] bool isSpawningThePieces = false;
    TalkingHead talkingHead;

    void Awake()
    {
      if (levelPiecesSpawned.Count <= 0) Debug.LogError("no spawned pieces in list. please assign at least one level piece to start generating from");
      walkwayGenerator = GetComponent<WalkwayGenerator>();
      andRailSpawner = GetComponent<AndRailSpawner>();
      thePieceSpawner = GetComponent<ThePieceSpawner>();
      talkingHead = FindObjectOfType<TalkingHead>();
    }

    public GameObject SpawnNextPiece(string word, bool startNewSentence)
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
        levelPiece = walkwayGenerator.AddPieceToWalkway(lastPieceFinalWalkoffPoint, word, startNewSentence);
      }

      levelPiecesSpawned.Add(levelPiece);

      if (talkingHead != null)
      {
        talkingHead.MoveToEndOfWalkway(levelPiece);
      }

      return levelPiece;
    }

    private Vector3 GetLastPieceFinalWalkoffPoint()
    {
      GameObject lastPiece = levelPiecesSpawned.Last();
      Vector3 lastPieceEndPoint = lastPiece.GetComponent<LevelPiece.Positioner>().GetFinalWalkOffPoint();
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
