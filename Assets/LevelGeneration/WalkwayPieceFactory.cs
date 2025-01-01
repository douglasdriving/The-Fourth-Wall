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

    public GameObject SpawnAboveTargetAndMoveIntoPlace(Vector3 finalPos, Quaternion finalRot, string pieceWord, float spawnHeightAboveTarget = 1.5f)
    {
      Vector3 spawnPos = finalPos + Vector3.up * spawnHeightAboveTarget;
      GameObject piece = Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      piece.transform.up = Vector3.back;
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      piece.GetComponent<LevelPiecePositioner>().MoveWithSimpleAnimation(finalPos, finalRot);
      return piece;
    }

    public GameObject InstantiateInFrontOfCameraAnMoveIntoPlace(string pieceWord, Vector3 targetPos, Quaternion targetRot)
    {
      GameObject piece = levelPieceMolds.CopyNextMold();
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      piece.GetComponent<LevelPiecePositioner>().MoveWithAnimation(targetPos, targetRot);
      return piece;
    }

    public GameObject InstantiateAtFinalPosition(Vector3 pos, Quaternion rot, string pieceWord)
    {
      GameObject piece = GameObject.Instantiate(walkwayPiecePrefab);
      piece.GetComponentInChildren<LevelPiecePositioner>().SetPosition(pos, rot);
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      return piece;
    }

    public float GetPieceHeight()
    {
      return walkwayPiecePrefab.transform.localScale.y;
    }
  }
}
