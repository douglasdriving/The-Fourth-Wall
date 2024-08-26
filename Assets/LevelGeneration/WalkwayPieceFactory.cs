using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

namespace LevelGeneration
{
  /// <summary>
  /// instantiates walkway pieces into the world
  /// </summary>
  public class WalkwayPieceFactory : MonoBehaviour
  {
    [SerializeField] GameObject walkwayPiecePrefab;
    [SerializeField] LevelPieceMolds levelPieceMolds;

    public GameObject InstantiatePieceAbovePos(Vector3 finalPos, Quaternion finalRot, Transform lastPiece, string pieceWord)
    {
      float heightAbovePrevious = 0.3f; //magic?
      Vector3 spawnPos = finalPos;
      spawnPos.y = lastPiece.position.y + heightAbovePrevious;
      GameObject piece = GameObject.Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      piece.transform.up = Vector3.back;
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      piece.GetComponent<LevelPiecePositioner>().MoveWithSimpleAnimation(finalPos, finalRot);
      return piece;
    }

    public GameObject InstantiateInFrontOfCamera(string pieceWord)
    {
      GameObject piece = levelPieceMolds.CopyNextMold();
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      return piece;
    }

    public GameObject InstantiateAtFinalPosition(Vector3 pos, Quaternion rot, string pieceWord)
    {
      GameObject piece = GameObject.Instantiate(walkwayPiecePrefab);
      piece.GetComponentInChildren<LevelPiecePositioner>().SetPosition(pos, rot);
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      return piece;
    }
  }
}
