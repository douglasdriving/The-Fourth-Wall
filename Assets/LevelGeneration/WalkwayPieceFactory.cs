using System;
using System.Collections;
using System.Collections.Generic;
using Player;
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

    public GameObject SpawnAboveTargetAndMoveIntoPlace(Vector3 finalPos, Quaternion finalRot, string pieceWord, Vector3 spawnPos, bool rotateTowardsPlayer)
    {
      GameObject piece = Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      if (rotateTowardsPlayer)
      {
        piece.transform.LookAt(Camera.main.transform.position);
        piece.transform.Rotate(-90, 0, 180);
      }
      else
      {
        piece.transform.up = Vector3.back;
      }
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(pieceWord);
      piece.GetComponent<LevelPiece.Positioner>().MoveWithSimpleAnimation(finalPos, finalRot);
      return piece;
    }

    public GameObject SpawnInFrontOfCameraAnMoveIntoPlace(string pieceWord, Vector3 targetPos, Quaternion targetRot)
    {
      GameObject piece = levelPieceMolds.CopyNextMold();
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(pieceWord);
      piece.GetComponent<LevelPiece.Positioner>().MoveWithAnimation(targetPos, targetRot);
      return piece;
    }

    public GameObject SpawnAtFinalPosition(Vector3 pos, Quaternion rot, string pieceWord)
    {
      GameObject piece = Instantiate(walkwayPiecePrefab);
      piece.GetComponent<LevelPiece.WordSetter>().SetWord(pieceWord);
      piece.GetComponentInChildren<LevelPiece.Positioner>().SetPosition(pos, rot);
      return piece;
    }

    public float GetPieceHeight()
    {
      return walkwayPiecePrefab.transform.localScale.y;
    }
  }
}
