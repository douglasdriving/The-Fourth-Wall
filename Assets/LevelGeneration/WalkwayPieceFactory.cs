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

    public GameObject SpawnAboveTargetAndMoveIntoPlace(Vector3 finalPos, Quaternion finalRot, string pieceWord, Vector3 spawnPos)
    {
      GameObject piece = Instantiate(walkwayPiecePrefab, spawnPos, Quaternion.identity);
      piece.transform.up = Vector3.back;
      piece.GetComponentInChildren<TMP_Text>().text = pieceWord;
      //should this be done here, or should there be a text setter IN the piece?
      //then, that text setter could also update the color of the piece, whenever a new word has been set.
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
